using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGoddess_Sword : MonoBehaviour
{

    //damage of the moving swords
    [SerializeField] int damage = 10;
    
    [SerializeField] float movementSpeed = 10f;
    
    bool isMoving = false;
    int dir = 1;

    void Start(){
        //find direction that sword is facing
        if (transform.rotation.eulerAngles.y == 0){
            dir = 1;
        }
        else {
            dir = -1;
        }
    }

    void Update(){
        
        //move sword forwards
        if (isMoving) {
            float movement = movementSpeed * Time.deltaTime;
            transform.position += movement * transform.right;

            //destroy the sword after it has traveled far enough
            if (Mathf.Abs(transform.position.x) > 20) {
                Destroy(gameObject);
            }
        }
    }

    //start moving the swords
    public void setMoving() {
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        
        //apply damage and knockback to the player if they collide with the swords
        if (other.CompareTag("Player")) {
            //apply X knockback
            other.GetComponent<PlayerInput>().modX = 4 * dir;
            other.GetComponent<PlayerInput>().modTimer = 0.5f;
            
            other.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback
            other.attachedRigidbody.velocity = new Vector2(other.attachedRigidbody.velocity.x, 3f);
        }
    }
}
