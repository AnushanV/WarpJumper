using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLines : MonoBehaviour{

    public GameObject linePrefab;

    //line parameters
    public float minY = -10f;
    public float maxY = 10f;
    public float delay = 2f;
    public float hitDetectionLimit = 0.1f;
    public float initialLineWidth = 0.3f;
    public float lineDuration = 1.1f;
    public float shrinkRate = 0.3f;

    public Vector3 lineOffset;

    void Start(){
        //generate lines in regular intervals
        InvokeRepeating("generateLine", delay, delay);
    }

    //called in regular intervals based on delay
    void generateLine(){
        
        //create a random y position based
        float randY = Random.Range(-minY, maxY);

        //instantiate the line and set the parameters
        GameObject line = Instantiate(linePrefab, new Vector3(transform.position.x, transform.position.y + randY, 0), Quaternion.identity) as GameObject;
        line.GetComponent<CreateLine>().hitDetectionLimit = hitDetectionLimit;
        line.GetComponent<CreateLine>().initialLineWidth = initialLineWidth;
        line.GetComponent<CreateLine>().shrinkRate = shrinkRate;
        line.GetComponent<CreateLine>().lineDuration = lineDuration;
    }

}
