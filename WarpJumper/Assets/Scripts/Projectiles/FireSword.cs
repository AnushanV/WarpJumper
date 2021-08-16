using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSword : MonoBehaviour
{

    public Animator animator;

    public float speed = 15;
    public int damage = 50;
    public Rigidbody2D rb;
    public Quaternion initAngle;

    public bool moving;
    public bool doneMoving = false;

    public float xDir = 1;
    public float yDir = 0;

    public float moveDuration = 0.2f;

    public float summonDuration = 3f;

    float straightSpeed;
    bool isStraight = false;

    bool passedEnemy = false;
    
    bool despawned = false;

    float angleToSword;
    Quaternion launchAngle;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        initAngle = transform.rotation;

        //let player know that a sword has been created
        GameObject.Find("Character").GetComponent<PlayerInput>().swordExists = true;

        //Move faster if moving in a straight horizontal line
        straightSpeed = Mathf.Sqrt(2 * (speed * speed));

    }

    private void Update(){
        
        if (!moving){
            if (doneMoving)
            {
                //stop rotation and movement
                transform.rotation = launchAngle;
                rb.velocity = new Vector2(0, 0);
                rb.isKinematic = true;
            }
            else {
                rotateSword(); //get user input to set rotation
            }
        }
        else {
            //apply movement speed based on if the sword is traveling straight or diagonally
            if (isStraight){
                rb.velocity = new Vector2(straightSpeed * xDir, straightSpeed * yDir);
            }
            else {
                rb.velocity = new Vector2(speed * xDir, speed * yDir);
            }

            //decrese travel time
            moveDuration -= Time.deltaTime;
        }

        //stop moving the sword after a certain duration
        if (moveDuration <= 0 && moving) {
            stopMoving();
        }

        //despawn sword after enough time has passed
        summonDuration -= Time.deltaTime;
        if (summonDuration <= 0 && !despawned) {
            despawned = true;
            disappear();
        }
        
    }
    
    //set rotation of the sword
    void rotateSword() {

        //get user input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(0, 0);

        float angle; //stores the angle of the sword

        //rotate angle of sword based on held direction to the nearest 45 degrees. This will allow digital and analog inputs to get the same angles
        if (x > 0){
            if (y > 0){
                angle = 45;
            }
            else if (y < 0){
                angle = -45;
            }
            else{
                angle = 0;
            }
        }
        else if (x < 0){
            if (y > 0){
                angle = 135;
            }
            else if (y < 0){
                angle = -135;
            }
            else{
                angle = 180;
            }
        }
        else{
            if (y > 0){
                angle = 90;
            }
            else if (y < 0){
                angle = -90;
            }
            else{
                angle = initAngle.eulerAngles.y;
            }
        }

        //apply the rotation to the sword
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    //start moving the sword
    public void startMoving() {
        moving = true;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        xDir = x;
        yDir = y;


        //Debug.Log("Launch direction: " + transform.localEulerAngles);
        
        //default direction
        if (x == 0 && y == 0) {
            xDir = 1;
            //move left if player facing left
            if (transform.localEulerAngles.z == 180){
                xDir = -1;
            }
            yDir = 0;
        }

        //sword is moving straight if x or y is 0
        if (y == 0 || x == 0) {
            isStraight = true;
        }

        launchAngle = transform.rotation; //store the launch angle
    }

    //stop moving the sword
    public void stopMoving() {

        //set values to stop sword from moving
        moving = false;
        doneMoving = true;
        speed = 0;
        rb.velocity = new Vector2(0, 0);

        //let player know that sword has stopped moving
        GameObject.Find("Character").GetComponent<PlayerInput>().casting = true;
        GameObject.Find("Character").GetComponent<PlayerInput>().swordTeleportAvailable = true;
    }

    //start the animation to make the sword despawn
    public void disappear() {
        animator.SetTrigger("disappear");

        //create particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("Teleport"), transform.position, transform.rotation) as GameObject;
        Destroy(particles, 0.5f);

        //let player know sword despawned and restore their sword availability
        PlayerInput player = GameObject.Find("Character").GetComponent<PlayerInput>();
        if (player != null) {
            player.swordExists = false;
            player.swordTeleportAvailable = false;
        }
        
    }

    //remove the sword from the game
    public void despawn() {
        GameObject character = GameObject.Find("Character");

        //restore user teleport if the sword has passed an enemy
        if(character != null && character.GetComponent<PlayerInput>().hasTeleport == false)
            character.GetComponent<PlayerInput>().hasTeleport = passedEnemy;
        
        Destroy(gameObject); //destroy the sword
    }

    //apply damage to enemies
    void OnTriggerEnter2D(Collider2D hitInfo){
        //Debug.Log(hitInfo.name);
        
        //damage enemies while moving
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null && !doneMoving){
            //show particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 0.3f);

            enemy.takeDamage(damage);
            passedEnemy = true; //record that sword 
        }
    }

}
