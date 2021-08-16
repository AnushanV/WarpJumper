using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineGenerator : MonoBehaviour
{

    [SerializeField] GameObject lineGeneratorPrefab;
    [SerializeField] Vector3 lineGeneratorOffset;
    bool spawned = false;

    private void OnTriggerEnter2D(Collider2D other){
        //start generating the lines when player collides with this object
        if (other.gameObject.CompareTag("Player")){

            if (!spawned){ //only allow line generator to be spawned once

                //spawn the line generator at the appropriate position
                GameObject lineGenerator = Instantiate(lineGeneratorPrefab, transform.position + lineGeneratorOffset, Quaternion.identity) as GameObject;
                
                //set parameters for the line generator
                lineGenerator.GetComponent<GenerateLines>().delay = 1.0f;
                lineGenerator.GetComponent<GenerateLines>().lineOffset = new Vector2(1, 0);
                lineGenerator.GetComponent<GenerateLines>().initialLineWidth = 0.2f;
                lineGenerator.GetComponent<GenerateLines>().shrinkRate = 0.2f;
                lineGenerator.GetComponent<GenerateLines>().lineDuration = 1.1f;

                spawned = true;
            }

        }
    }
}
