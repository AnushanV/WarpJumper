using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGoddess_Movement : MonoBehaviour{

    public Transform player;

    Vector2 initialPos;
    
    public int dir = -1; //initial direction

    void Start(){
        initialPos = transform.position; //store the starting position
    }

    void Update(){

        //flip sprite towards the player
        if (player != null) {
            
            if (transform.position.x < player.position.x){
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                dir = -1;
            }
            else{
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                dir = 1;
            }
        }
    }

    //teleport behind the player
    public void teleportToPlayer() {
        if (player != null) {

            //get x position behind player
            float newXPos = player.position.x;
            if (dir == -1){
                newXPos += 2f;
            }
            else{
                newXPos -= 2f;
            }

            //create particle effect
            GameObject particles = GameObject.Instantiate(Resources.Load("Teleport"), transform.position + new Vector3(0, -2.5f, 0), transform.rotation) as GameObject;
            particles.transform.localScale = new Vector3(4, 4, 4);
            Destroy(particles, 0.5f);

            //teleport behind player
            transform.position = new Vector2(newXPos, transform.position.y);
        }
    }

    //teleport to the initial position
    public void teleportToInitial() {
        //create particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("Teleport"), transform.position, transform.rotation) as GameObject;
        particles.transform.localScale = new Vector3(4, 4, 4);
        Destroy(particles, 0.5f);
        transform.position = initialPos;
    }
}
