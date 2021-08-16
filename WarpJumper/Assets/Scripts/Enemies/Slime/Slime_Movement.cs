using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Movement : MonoBehaviour{

    [SerializeField] Animator animator;
    [SerializeField] LayerMask players;
    [SerializeField] Rigidbody2D rb;

    //movement variables
    [SerializeField] float movementSpeed = 0.5f;
    public int direction;

    //control if enemy is moving
    public bool stunned = false;
    public bool stopped = false;

    //timer until enemy stops moving
    float stopTimer;
    float stopDuration = 8;

    //range for time interval between stopping
    int minTimer = 1;
    int maxTimer = 7;

    void Start(){

        //pick a random initial direction
        if (Random.Range(-1, 1) < 0){
            direction = -1;
        }
        else{
            direction = 1;
        }

        //randomly stop movement
        stopTimer = Random.Range(minTimer, maxTimer + 1);
        
    }

    void Update(){
        
        float movementX = 0;
        
        //move if slime is able to move
        if (stunned || stopped){
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }
        else{
            movementX = movementSpeed * direction;
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }

        animator.SetFloat("speed", Mathf.Abs(movementX));

        //adjust movement timers based on current state of slime
        if (stopped){
            stopDuration -= Time.deltaTime;
        }
        else{
            stopTimer -= Time.deltaTime;
        }
        if (stopTimer <= 0 && !stopped){
            stopped = true;
            stopDuration = Random.Range(minTimer, maxTimer + 1);
        }
        if (stopDuration <= 0 && stopped){
            stopped = false;
            stopTimer = Random.Range(minTimer, maxTimer + 1);
        }

        //flip sprite based on direction
        if (direction < 0f){
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction > 0f){
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //apply stun to slime
    void stun(){
        stunned = true;
    }

    //remove stun from slime
    void removeStun(){
        stunned = false;
    }

    //flip movement if colliding with enemy boundary
    void OnTriggerEnter2D(Collider2D other){
        //flip movement
        if (other.CompareTag("EnemyBoundary") && !stunned){
            direction *= -1;
        }
    }
}
