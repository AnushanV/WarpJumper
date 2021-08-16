using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : MonoBehaviour
{

    public GameObject player;
    public bool facingRight = false;
    public Animator animator;
    [SerializeField] private Vector3 destination = Vector3.zero;


    //attack related
    public int currentAttack = 0; //0 = idle, 1 = dash attack, 2 = projectiles, 3 = idle
    public int numAttacks = 4; //total number of attacks

    public float attackInterval = 6f;
    public float movementSpeed = 6f;
    public float runMovementSpeed = 6f;
    public float dashAttackMovementSpeed = 6f;
    public int damage = 30;

    public bool canTurn = true;
    public bool destinationReached = false;

    public GameObject icePick;
    public GameObject iceCastAnim;

    public LayerMask players;
    //hitbox parameters for sword swing
    public float attackOffsetX = 3f;
    public float attackOffsetY = -2.5f;
    public float attackRange = 4f;

    public int dir = 1;

    bool isDead = false;

    private void Start()
    {
        InvokeRepeating("changeAttack", attackInterval, attackInterval);
    }


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            //flip sprite depending on where player is
            if (canTurn) {
                //find position of player
                float xDiff = transform.position.x - player.transform.position.x;
                float yDiff = transform.position.y - player.transform.position.y;

                //flip sprite
                if (xDiff <= 0)
                {
                    dir = -1;
                    facingRight = true;
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    dir = 1;
                    facingRight = false;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }

            if (currentAttack == 1) {

                if (!destinationReached) {
                    transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
                }

                //end run animation
                if ((Vector3.Distance(transform.position, destination) < 0.2f) && !destinationReached){
                    animator.SetTrigger("idle");
                    canTurn = true;
                    destinationReached = true;
                }

                //attack if near player
                if ((Mathf.Abs(transform.position.x - player.transform.position.x) < 1f) && !destinationReached)
                {
                    movementSpeed = dashAttackMovementSpeed;
                    animator.SetTrigger("dashAttack");
                    destinationReached = true;
                }
            }

            if (currentAttack == 2) { 
            
            }

        }
    }

    public void endDashAttack() {
        animator.SetTrigger("idle");
        canTurn = true;
        destinationReached = true;
    }

    public void endCast() {
        animator.SetTrigger("idle");
        canTurn = true;
    }

    public void castAttack() {
        Debug.Log("casting ice picks");
        Instantiate(iceCastAnim, transform.position, Quaternion.identity);
        StartCoroutine(spawnIcePicks());
    }

    IEnumerator spawnIcePicks(){
        for (int i = 0; i < 10; i++){
            Instantiate(icePick, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void changeAttack() {
        canTurn = true;

        //return to idle from attack 1
        if (currentAttack == 1) {
            if (!destinationReached) {
                animator.SetTrigger("idle");
            }
        }

        currentAttack++;
        if (currentAttack % numAttacks == 0) {
            currentAttack = 0;
        }

        //set destination for attack 1
        if (currentAttack == 1) {
            movementSpeed = runMovementSpeed;
            destinationReached = false;
            canTurn = false;
            animator.SetTrigger("run");
            if (facingRight)
            {
                destination = new Vector3(player.transform.position.x + 10, transform.position.y, 0);
            }
            else
            {
                destination = new Vector3(player.transform.position.x - 10, transform.position.y, 0);
            }
        }

        if (currentAttack == 2) {
            canTurn = false;
            animator.SetTrigger("cast");
        }
    }

    void hitbox(){

        //create a hitbox when the sword swings
        float dir = -GetComponent<WarriorBehaviour>().dir;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position + new Vector3(attackOffsetX * dir, attackOffsetY, 0), attackRange, players);

        //apply knockback and damage to player if they are in the hitbox
        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("Enemy hit" + player.name);

            //apply X knockback
            player.GetComponent<PlayerInput>().modX = 10 * dir;
            player.GetComponent<PlayerInput>().modTimer = 0.5f;

            player.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback
            player.attachedRigidbody.velocity = new Vector2(player.attachedRigidbody.velocity.x, 10f);
        }

    }

    //hitbox visualization
    private void OnDrawGizmosSelected()
    {
        float dir = -GetComponent<WarriorBehaviour>().dir;
        Gizmos.DrawWireSphere(transform.position + new Vector3(attackOffsetX * dir, attackOffsetY, 0), attackRange);
    }
}
