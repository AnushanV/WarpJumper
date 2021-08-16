using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateExplosion : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] LayerMask players;

    [SerializeField] float attackRange = 5f;
    [SerializeField] float attackOffsetY = 5f;
    
    void Start(){

        InvokeRepeating("applyDamage", 1f, 0.3f); //apply damage every .3 seconds

        Quaternion particleRotation = Quaternion.Euler(-90, 0, 0); //set rotation for particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("Explosion"), transform.position, particleRotation) as GameObject;

        //remove particle effect and this object after 10 seconds
        Destroy(particles, 8f);
        Destroy(gameObject, 8f);
    }

    //apply damage to player
    void applyDamage(){

        //create circle hitbox
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, attackOffsetY, 0), attackRange, players);

        //apply damage to player
        foreach (Collider2D player in hitPlayers){
            //Debug.Log("Explosion hit" + player.name);
            player.GetComponent<PlayerHealth>().takeDamage(damage);

        }

    }

    //hitbox visualization
    private void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, attackOffsetY, 0), attackRange);
    }
}
