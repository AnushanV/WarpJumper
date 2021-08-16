using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1_Attack : MonoBehaviour{

    public Animator animator;
    public LayerMask players;

    //range of time between attacks
    [SerializeField] float minAttackTimer = 3;
    [SerializeField] float maxAttackTimer = 8;

    //keep track of monster actions
    bool aggro = false;
    bool attacking = false;

    //height and width of rectangular hitbox
    [SerializeField] int attackRangeX = 8;
    [SerializeField] int attackRangeY = 2;

    //offset position of hitbox
    [SerializeField] int attackOffsetX = 4;
    [SerializeField] int attackOffsetY = -2;

    //knockback effect of monster's attack
    [SerializeField] int knockbackX = 6;
    [SerializeField] int knockbackY = 8;
    [SerializeField] float knockbackDuration = 0.8f;

    //time until next attack
    float attackTimer;

    //damage dealt by attack
    public int damage = 30;

    
    void Start(){
        //set an initial timer until next attack
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
    }

    void Update(){

        
        if (aggro){ //when player is near enemy
            
            //stop movement
            this.GetComponent<Monster_Movement>().stopped = true;
            
            //decrease attack timer or perform attack based on the timer
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
                if (GameObject.Find("Character").GetComponent<PlayerInput>().transform.position.x > transform.position.x ){
                    this.GetComponent<Monster_Movement>().direction = 1;
                }
                else{
                    this.GetComponent<Monster_Movement>().direction = -1;
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //decrease the attack timer when player is close
        if (other.CompareTag("Player")){
            aggro = true;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        
        //reset the attack timer to a random value in the range
        if (other.CompareTag("Player")){
            aggro = false;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);

            //pick a random direction to move
            if (Random.Range(-1, 1) < 0){
                this.GetComponent<Monster_Movement>().direction = -1;
            }
            else{
                this.GetComponent<Monster_Movement>().direction = 1;
            }
        }
    }

    //create a rectangle that looks for a player in order to deal damage and knockback
    void hitbox(){

        //create the rectangle
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackOffsetX * this.GetComponent<Monster_Movement>().direction, attackOffsetY, 0), new Vector2(attackRangeX, attackRangeY), 0f, players);

        //apply damage and knockback to player
        foreach (Collider2D player in hitPlayers){
            Debug.Log("Monster_Movement hit" + player.name);

            //apply X knockback
            player.GetComponent<PlayerInput>().modX = knockbackX * this.GetComponent<Monster_Movement>().direction;
            player.GetComponent<PlayerInput>().modTimer = knockbackDuration;

            player.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback
            player.attachedRigidbody.velocity = new Vector2(player.attachedRigidbody.velocity.x, knockbackY);
        }

    }

    //hitbox visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(attackOffsetX * this.GetComponent<Monster_Movement>().direction, attackOffsetY, 0), new Vector3(attackRangeX, attackRangeY, 1));
    }

}
