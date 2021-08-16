using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public LayerMask enemyLayers;

    public Transform attackPoint1;
    public float attackRange1 = 0.6f;
    public int damage1 = 20;

    public Transform attackPoint2;
    public float attackRange2 = 0.75f;
    public int damage2 = 20;

    public Transform attackPoint3;
    public float attackRange3 = 0.5f;
    public int damage3 = 40;

    private Transform[] attackPoints = new Transform[3];
    private float[] attackRanges = new float[3];
    private int[] damages = new int[3];

    [SerializeField] AudioSource swing;

    void Start(){

        //center positions of hitbox for each attack
        attackPoints[0] = attackPoint1;
        attackPoints[1] = attackPoint2;
        attackPoints[2] = attackPoint3;

        //ranges of hitbox for each attack
        attackRanges[0] = attackRange1;
        attackRanges[1] = attackRange2;
        attackRanges[2] = attackRange3;

        //damage dealt for each attack
        damages[0] = damage1;
        damages[1] = damage2;
        damages[2] = damage3;
    }


    public void attack(int attackNum) {
        
        //create the appropriate hitbox based on the attack
        if (attackNum == 1){
            hitbox(attackPoint1, attackRange1, damage1);
        }
        else if (attackNum == 2){
            hitbox(attackPoint2, attackRange2, damage2);
        }
        else if (attackNum == 3){
            hitbox(attackPoint3, attackRange3, damage3);
        }

    }

    //create a hitbox to damage enemies
    void hitbox(Transform attackPoint, float attackRange, int damage) {

        swing.Play(); //play the sword swing sound effect

        //create a circle hitbox
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //apply damage to enemies inside the hitbox
        foreach (Collider2D enemy in hitEnemies) {
            Debug.Log("Player hit: " + enemy.name);
            enemy.GetComponent<Enemy>().takeDamage(damage);
            
            //show damage particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 0.3f);
        }

    }

    /*
    private void OnDrawGizmosSelected()
    {
        if (attackPoint3 == null)
            return;
        Gizmos.DrawWireSphere(attackPoint3.position, attackRange3);
    }
    */
}
