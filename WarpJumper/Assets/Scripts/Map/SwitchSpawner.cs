using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSpawner : MonoBehaviour
{
    public GameObject switchToSpawn;

    // Start is called before the first frame update
    void Start(){
        switchToSpawn.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        //set switch active when all children are gone
        if (transform.childCount <= 0) {
            switchToSpawn.SetActive(true);
            Destroy(gameObject);
        }
    }
}
