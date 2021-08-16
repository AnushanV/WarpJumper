using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Movement : MonoBehaviour{

    [SerializeField] Animator animator;
    [SerializeField] LayerMask players;
    [SerializeField] Rigidbody2D rb;

    //movement variables
    [SerializeField] float movementSpeed = 0.5f;
    public int direction;

    //status of enemy
    public bool stunned = false;
    public bool stopped = false;

    //timer to stand still
    float stopTimer;
    float stopDuration = 8;

    //range of time to stop for
    int minTimer = 1;
    int maxTimer = 7;

    void Start(){
    
        //pick a random direction to walk in
        if (Random.Range(-1, 1) < 0){
            direction = -1;
        }
        else{
            direction = 1;
        }

        //randomly stop movement
        stopTimer = Random.Range(minTimer, maxTimer + 1);
        //Debug.Log("Stop Timer: " + stopTimer);
    }

    // Update is called once per frame
    void Update(){
        
        float movementX = 0;
        
        //Move enemy if they are able to move
        if (stunned || stopped || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){

            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }
        else{
            movementX = movementSpeed * direction;
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }
        animator.SetFloat("speed", Mathf.Abs(movementX));

        //decrease time until monster can move
        if (stopped){
            stopDuration -= Time.deltaTime;
        }
        else{
            stopTimer -= Time.deltaTime;
        }

        //decrease time until monster stops moving
        if (stopTimer <= 0 && !stopped){
            stopped = true;
            stopDuration = Random.Range(minTimer, maxTimer + 1);
        }
        if (stopDuration <= 0 && stopped){
            stopped = false;
            stopTimer = Random.Range(minTimer, maxTimer + 1);
        }

        //Flip sprite based on direction
        if (direction < 0f){
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction > 0f){
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //stop movement of monster when stunned
    void stun(){
        stunned = true;
    }

    //resume movement of monster after stunned
    void removeStun(){
        stunned = false;
    }

    //flip direction when meeting an enemy border
    void OnTriggerEnter2D(Collider2D other){

        //flip movement
        if (other.CompareTag("EnemyBoundary") && !stunned){
            direction *= -1;
        }

    }
}
