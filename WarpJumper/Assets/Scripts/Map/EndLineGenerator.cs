using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineGenerator : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other){
        
        //destroy the line generator when player collides with this object
        if (other.CompareTag("Player")){
            GameObject[] lineGenerators = GameObject.FindGameObjectsWithTag("LineGenerator");
            foreach (GameObject lineGenerator in lineGenerators) {
                Destroy(lineGenerator);
            }
        }
    }
}
