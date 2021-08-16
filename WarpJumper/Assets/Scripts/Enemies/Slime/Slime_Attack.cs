using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Attack : MonoBehaviour{

    public Animator animator;
    public LayerMask players;
    
    //damage dealt by slime attack
    public int damage = 20;

    //time interval range between attacks
    [SerializeField] float minAttackTimer = 1;
    [SerializeField] float maxAttackTimer = 2;

    //slime state
    bool aggro = false;
    bool attacking = false;

    //hitbox parameters
    [SerializeField] int attackRange = 1;
    [SerializeField] int attackOffsetX = 0;
    [SerializeField] int attackOffsetY = 0;

    //timer for next attack
    float attackTimer;


    void Start(){
        //set time until next attack
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
    }

    void Update(){
        
        //attack if near player
        if (aggro){

            //stop movement of slime
            this.GetComponent<Slime_Movement>().stopped = true;
            
            //attack or decrease timer based on the current timer
            if (attackTimer <= 0 && !attacking){
                attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
                attacking = true;
                animator.SetTrigger("attack");
            }
            else{
                attacking = false;
                attackTimer -= Time.deltaTime;
            }

            //flip towards player
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                if (GameObject.Find("Character").GetComponent<PlayerInput>().transform.position.x > transform.position.x){
                    this.GetComponent<Slime_Movement>().direction = 1;
                }
                else{
                    this.GetComponent<Slime_Movement>().direction = -1;
                }
            }
            
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        
        //attack and set aggro if player comes near
        if (other.CompareTag("Player")){
            aggro = true;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
            //attack on entry
            //animator.SetTrigger("attack");
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        
        //remove aggro when player is far away
        if (other.CompareTag("Player")){
            aggro = false;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);

            //move in random direction
            if (Random.Range(-1, 1) < 0){
                this.GetComponent<Slime_Movement>().direction = -1;
            }
            else{
                this.GetComponent<Slime_Movement>().direction = 1;
            }
        }

    }

    //Apply damage and knockback to nearby player
    void hitbox(){
        //create hitbox circle
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position + new Vector3(attackOffsetX * this.GetComponent<Slime_Movement>().direction, attackOffsetY, 0), attackRange, players);

        //apply damage and knockback if player is inside circle
        foreach (Collider2D player in hitPlayers){
            Debug.Log("Enemy hit" + player.name);

            //apply X knockback
            player.GetComponent<PlayerInput>().modX = 4 * this.GetComponent<Slime_Movement>().direction;
            player.GetComponent<PlayerInput>().modTimer = 0.5f;

            player.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback
            player.attachedRigidbody.velocity = new Vector2(player.attachedRigidbody.velocity.x, 3f);
        }

    }

    //hitbox visualization
    private void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(transform.position + new Vector3(attackOffsetX * this.GetComponent<Slime_Movement>().direction, attackOffsetY, 0), attackRange);
    }

}
