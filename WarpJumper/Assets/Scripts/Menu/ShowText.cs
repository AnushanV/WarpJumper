using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{

    [SerializeField] GameObject signInfo;

    private void Start(){
        //hide the sign info
        signInfo.SetActive(false);
    }

    //show the text when player gets close to the sign
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            //Debug.Log("IN");
            signInfo.SetActive(true);
        }
        
    }

    //hide the text when player leaves sign area
    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            //Debug.Log("OUT");
            signInfo.SetActive(false);
        }
    }


}
