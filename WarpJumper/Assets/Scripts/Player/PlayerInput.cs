using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerInput : MonoBehaviour{
    
    private Rigidbody2D rb;
    private Animator controller;
    public Transform swordPoint;
    public GameObject swordPrefab;
    
    //movement particles
    [SerializeField] ParticleSystem dustParticles;
    private ParticleSystem.EmissionModule dustEmission;

    //movement attributes
    [SerializeField] private float groundSpeed = 5.0f;
    [SerializeField] private float airSpeed = 8.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float doubleJumpForce = 5.0f;
    [SerializeField] private float jumpMod = 1.2f;
    [SerializeField] private float fallMod = 1.0f;

    [SerializeField] private Transform footPosition;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float closeEnough = 0.1f;
    

    [SerializeField] private bool hasDoubleJump = false;
    public bool hasTeleport = false;

    //current movement
    float movementX = 0f;
    float movementY = 0f;

    //attacking states
    bool attacking = false;
    bool attacking2 = false;
    bool attacking3 = false;
    bool firingBow = false;
    public bool casting = false;

    //attack condition
    public bool swordExists = false;
    public bool swordTeleportAvailable = false;

    //knockback modifiers
    public float modX = 0f;
    public float modY = 0f;
    public float modTimer = 0;

    //sound effects
    [SerializeField] AudioSource warp;
    [SerializeField] AudioSource step;
    [SerializeField] AudioSource throwSword;
    [SerializeField] AudioSource jump;

    //arrow ability
    public int currentArrow = 0;
    public int numArrows = 1;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Animator>();
    }

    private void Start(){
        //hide the dust particles initially
        dustEmission = dustParticles.emission;
        dustEmission.enabled = false;
    }

    private void Update(){

        //check if ray fired downwards touches ground
        RaycastHit2D hit = Physics2D.Raycast(footPosition.position, Vector2.down, closeEnough, groundLayers);
        
        //set values based on if player is grounded
        if (hit.collider){
            isGrounded = true;

            //restore resources
            hasDoubleJump = true;
            hasTeleport = true;

            //set animator parameters
            controller.SetBool("isGrounded", true);
            controller.SetBool("isRising", false);
            controller.SetBool("isFalling", false);
        }
        else{
            isGrounded = false;
            controller.SetBool("isGrounded", false);
        }
        
        //perform player actions
        xMovement();
        yMovement();
        attacks();

        //update arrow type if unlocked
        if (SceneManager.GetActiveScene().buildIndex >= 5) {
            updateArrowType();
        }
        


        //Flip sprite if needed
        if (movementX < 0f){
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (movementX > 0f){
            //transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //reset knockback mods when timer runs out
        if (modTimer <= 0)        {
            modX = 0;
            modY = 0;
        }
        else {
            modTimer -= Time.deltaTime; //reduce knockback duration
        }

        //apply horizontal velocity mods
        rb.velocity = new Vector2(rb.velocity.x + modX, rb.velocity.y);

    }

    //perform attacks
    private void attacks() {
        threeHitCombo(); //sword combo
        fireBow(); //shoot arrows

        //check when cast ends
        if (controller.GetCurrentAnimatorStateInfo(0).IsName("Cast")){
            casting = true;
        }
        else{
            casting = false;
        }

        if (casting){
            //disable horizontal movement
            rb.velocity = new Vector2(0, rb.velocity.y);

            //decrease fall speed
            rb.drag = 100;
        }
        else{
            rb.drag = 0; //set fall speed back to normal
        }

        //allow user to fire sword if they have it available
        if (hasTeleport) {
            cast();
        }
        
    }


    private void xMovement() {
        float x = Input.GetAxis("Horizontal"); //get input

        dustEmission.enabled = false; //hide movement particles by default
        step.volume = 0; //mute step sound
        
        //grounded movement
        if (isGrounded) {
            if (x != 0){
                dustEmission.enabled = true; //show movement particles
                step.volume = 0.3f; //play walking sound
            }

            //set movemement
            movementX = x * groundSpeed;
            controller.SetFloat("groundSpeed", Mathf.Abs(movementX));
        }
        //air movement
        else {
            movementX = x * airSpeed;
            controller.SetFloat("groundSpeed", 0);
        }

        //set the velocity based on calculated movement
        rb.velocity = new Vector2(movementX, rb.velocity.y);
    }

    private void yMovement() {

        //check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded && !attacking){
            //apply jump force
            movementY = jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, movementY);
            jump.Play(); //play jump sound
        }

        //check if player can double jump
        if (Input.GetButtonDown("Jump") && hasDoubleJump && !isGrounded){
            
            //apply double jump force
            controller.SetTrigger("doubleJump");
            hasDoubleJump = false;
            movementY = doubleJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, movementY);

            //show double jump effect
            GameObject particles = GameObject.Instantiate(Resources.Load("DoubleJumpDust"), transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
            Destroy(particles, 2.5f);

            jump.Play(); //play jump sound
        }

        //Reduce jump floatiness
        //taken from Better Jumping in Unity With Four Lines of Code by Board to Bits Games
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity * fallMod * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity * jumpMod * Time.deltaTime;
        }

        //set animator parameters
        if (!isGrounded && rb.velocity.y > 0){
            controller.SetBool("isRising", true);
            controller.SetBool("isFalling", false);
        }
        else if (!isGrounded) {
            controller.SetBool("isRising", false);
            controller.SetBool("isFalling", true);
        }

    }

    private void threeHitCombo() {

        //start attack if it is available
        if (Input.GetButtonDown("Fire2") && (controller.GetCurrentAnimatorStateInfo(0).IsName("Idle") || controller.GetCurrentAnimatorStateInfo(0).IsName("Walk"))){
            controller.SetTrigger("attack1");
            controller.SetBool("attacking", true);
        }

        //continue combo if user presses attack button again in the middle of the combo
        if (attacking){

            //stop x movement
            movementX = 0; 
            rb.velocity = new Vector2(movementX, rb.velocity.y); 

            //check for 2nd combo input
            if (Input.GetButtonDown("Fire2") && controller.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !attacking2){
                controller.SetTrigger("attack2");
                attacking2 = true;
            }

            //check for 3rd combo input
            if (Input.GetButtonDown("Fire2") && controller.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !attacking3){
                controller.SetTrigger("attack3");
                attacking3 = true;
            }

            //allow user to move during 3rd hit of combo
            if (controller.GetCurrentAnimatorStateInfo(0).IsName("Attack3")){
                float x = Input.GetAxis("Horizontal");
                movementX = 7 * x;
                rb.velocity = new Vector2(movementX, rb.velocity.y);
            }
        }

        //check when attack ends
        if (controller.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || controller.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            || controller.GetCurrentAnimatorStateInfo(0).IsName("Attack3")){
            attacking = true;
        }
        else{
            //reset attacking states
            attacking = false;
            attacking2 = false;
            attacking3 = false;
            controller.SetBool("attacking", false);
        }
    }

    private void cast(){

        //summon sword
        if (Input.GetButtonDown("Fire3") && (controller.GetCurrentAnimatorStateInfo(0).IsName("Idle") || controller.GetCurrentAnimatorStateInfo(0).IsName("Walk") //grounded
            || controller.GetCurrentAnimatorStateInfo(0).IsName("Rising") || controller.GetCurrentAnimatorStateInfo(0).IsName("Falling") || controller.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump")) //aerial
            && !swordExists) {

            //play cast animation and summon sword
            controller.SetTrigger("cast");
            Instantiate(swordPrefab, swordPoint.position, swordPoint.rotation);
            throwSword.Play(); //play sound effect
        }

        //teleport to sword
        if (Input.GetButtonDown("Fire3") && (controller.GetCurrentAnimatorStateInfo(0).IsName("Idle") || controller.GetCurrentAnimatorStateInfo(0).IsName("Walk") //grounded
           || controller.GetCurrentAnimatorStateInfo(0).IsName("Rising") || controller.GetCurrentAnimatorStateInfo(0).IsName("Falling") || controller.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump")) //aerial
           && (swordExists && swordTeleportAvailable)){
            controller.SetTrigger("teleport");
        }

    }


    private void fireBow() {

        
        if (Input.GetButton("Fire1") && (controller.GetCurrentAnimatorStateInfo(0).IsName("Idle") || controller.GetCurrentAnimatorStateInfo(0).IsName("Walk") //grounded
            || controller.GetCurrentAnimatorStateInfo(0).IsName("Rising") || controller.GetCurrentAnimatorStateInfo(0).IsName("Falling"))){  //aerial
            controller.SetTrigger("fireBow"); //shoot arrows
        }


        if (firingBow){
            //allow movement while firing bow
            float x = Input.GetAxis("Horizontal");
            movementX = x * groundSpeed;

            //lower movement speed while firing bow
            
            if (currentArrow == 1)
            {
                controller.speed = 0.7f;
                //movementX *= 0.25f;
            }
            

            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }

        //check when attack ends
        if (controller.GetCurrentAnimatorStateInfo(0).IsName("Bow") || controller.GetCurrentAnimatorStateInfo(0).IsName("AirBow")){
            firingBow = true;
        }
        else{
            firingBow = false;
            controller.speed = 1f;
        }

    }

    //create hitbox when called from the animation event
    private void createHitbox(int comboNum) { 
        //create the hitbox for the corresepending attack in the combo
        if (comboNum == 1){
            this.GetComponent<Sword>().attack(1);
        }
        else if (comboNum == 2){
            this.GetComponent<Sword>().attack(2);
        }
        else if (comboNum == 3){
            this.GetComponent<Sword>().attack(3);
        }
    }

    //fire an arrow
    private void createArrow() {
        this.GetComponent<Bow>().fireArrow();
    }
    
    //teleport to the sword
    public void startTeleport() {
        
        GameObject[] swords;
        swords = GameObject.FindGameObjectsWithTag("Sword");

        //teleport to sword and remove it
        if (swords[0] != null) {
            swordTeleportAvailable = false;
            int boostRadius = 2; //minimum distance away from sword to get a movement boost

            //maximum values to modify velocity by
            int modYCap = 10;
            int modXCap = 3;

            //set velocity mods based on position relative to the sword
            if (this.transform.position.y + boostRadius < swords[0].transform.position.y){
                modY = 7 + (this.transform.position.y + boostRadius - swords[0].transform.position.y);
                if (modY > modYCap) {
                    modY = modYCap;
                }
            }
            else {
                modY = 0;
            }
            if (this.transform.position.x - boostRadius > swords[0].transform.position.x){
                modX = -1 - (this.transform.position.x - boostRadius - swords[0].transform.position.x);
                if (modX < -modXCap){
                    modX = -modXCap;
                }
            }
            else if (this.transform.position.x + boostRadius < swords[0].transform.position.x){
                modX = 1 + (swords[0].transform.position.x - this.transform.position.x + boostRadius);
                if (modX > modXCap)
                {
                    modX = modXCap;
                }
            }
            else {
                modX = 0;
            }

            //duration for the movement boost
            modTimer = 0.5f;

            swords[0].GetComponent<FireSword>().disappear(); //despawn the sword after teleporting to it

            //create particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("Teleport"), transform.position, transform.rotation) as GameObject;
            Destroy(particles, 0.5f);

            //move to the sword
            this.transform.position = swords[0].transform.position;
            
            //reset velocity
            rb.velocity = new Vector2(0, modY);

            //reset double jump
            hasDoubleJump = true;
            hasTeleport = false;

            //play sound
            warp.Play();
        }
    }

    private void updateArrowType() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            currentArrow++;
            currentArrow = currentArrow % (numArrows + 1);
        }
    }
}
