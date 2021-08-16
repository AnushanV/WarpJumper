using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 8; //speed of fireball
    public int damage = 30; //damage of fireball
    public float duration = 3f; //duration of fireball
    public float rotationSpeed = 200f; //rotation speed of fireball
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        //move fireball forward
        rb.velocity = transform.right * speed;
        rb.angularVelocity = rotationSpeed * transform.right.x;
        Destroy(gameObject, duration);
    }

    void Update() {
        //update the velocity
        rb.velocity = transform.right * speed;
    }

    //apply damage to enemies
    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        //Debug.Log(hitInfo.name);

        //apply damage to enemies
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.takeDamage(damage);

            //display particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 0.3f);
        }
    }
}
