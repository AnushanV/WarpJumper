using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGoddess_Attack : MonoBehaviour
{

    [SerializeField] Animator animator;
    public GameObject swordPrefab;
    public GameObject linePrefab;
    public GameObject explosionPrefab;
    public LayerMask players;

    //damage dealt by sword swings
    [SerializeField] int damage = 20;

    int attackNum = 0; //current attack in loop
    public float attackInterval = 4f; //time between attacks
    float attackTimer; //time until next attack
    bool attackAvailable = false; //check if able to attack

    //hitbox parameters for sword swing
    public float attackOffsetX = 3f;
    public float attackOffsetY = -2.5f;
    public float attackRange = 4f;

    //direction to attack for the 3rd attack
    int attack3Direction = -1;

    void Start(){
        attackTimer = attackInterval; //set timer for first attack
    }

    // Update is called once per frame
    void Update(){

        //perform the appropriate attack in the loop when an attack is available
        if (attackAvailable){
            if (attackNum == 0){
                animator.SetTrigger("attack1");
            }
            else if (attackNum == 1){
                animator.SetTrigger("attack2");
            }
            else {
                animator.SetTrigger("attack3");
            }

            attackAvailable = false;
        }

        //decrease timers if in the idle state
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
            //manage attack timer
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                attackAvailable = true;
                attackTimer = attackInterval;

                //update current attackNumber
                attackNum = (attackNum + 1) % 3;
            }
        }

    }

    //attack methods called from animator
    void attack1() {
        
        Debug.Log("Attack 1");

        //Create an explosion on alternating sides of the map
        Vector3 explosionPos = new Vector3(14 * attack3Direction, -7, 0);
        Instantiate(explosionPrefab, explosionPos, Quaternion.identity);

        //flip direction for next attack
        attack3Direction *= -1;
    }

    void attack2(){

        Debug.Log("Attack 2");
        
        //set x position for swords behind boss
        float dir = GetComponent<SunGoddess_Movement>().dir;
        float x = transform.position.x + (3 * dir);
        
        Quaternion swordRotation;

        //set rotation of the swords
        if (dir == 1){
            swordRotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            swordRotation = Quaternion.Euler(0, 0, 0);
        }

        float[] ys = {-5.5f, -3.5f, -1.5f, 0.5f}; //y positions for the swords

        //instantiate 4 swords behind boss
        for (int i = 0; i < ys.Length; i++) {
            Instantiate(swordPrefab, new Vector2(x, ys[i]), swordRotation);
        }
    }

    void attack3(){
        Debug.Log("Attack 3");
    }

    void hitbox(){

        //create a hitbox when the sword swings
        float dir = -GetComponent<SunGoddess_Movement>().dir;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position + new Vector3(attackOffsetX * dir, attackOffsetY, 0), attackRange, players);
        
        //apply knockback and damage to player if they are in the hitbox
        foreach (Collider2D player in hitPlayers){
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
    private void OnDrawGizmosSelected(){
        float dir = -GetComponent<SunGoddess_Movement>().dir;
        Gizmos.DrawWireSphere(transform.position + new Vector3(attackOffsetX * dir, attackOffsetY, 0), attackRange);
    }

}
