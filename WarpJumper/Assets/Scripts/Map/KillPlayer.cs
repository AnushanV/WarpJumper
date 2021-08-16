//https://www.youtube.com/watch?v=H69PfxOr6bk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour{
    
    public int respawnScene; //spawn to respawn on

    void Start(){
        //get the current scene
        respawnScene = SceneManager.GetActiveScene().buildIndex;
    }

    //apply damage equal to the player's health when they touch this object
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerHealth>().takeDamage(other.GetComponent<PlayerHealth>().maxHealth);
        }
    }
}
