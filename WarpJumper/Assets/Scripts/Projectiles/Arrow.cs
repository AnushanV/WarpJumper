using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour{

    public float speed = 8; //speed of arrow
    public int damage = 10; //damage of arrow
    [SerializeField] Rigidbody2D rb;

    void Start(){
        //move arrow forward
        rb.velocity = transform.right * speed;
    }

    //apply damage to enemies
    void OnTriggerEnter2D(Collider2D hitInfo) {

        //Debug.Log(hitInfo.name);
        
        //apply damage to enemies
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.takeDamage(damage);

            //display particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 0.3f);
        }

        Destroy(gameObject); //destroy the arrow when it collides with an object

    }
}
