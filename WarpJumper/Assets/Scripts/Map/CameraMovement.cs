using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour{

    public Transform player;
    public Vector3 cameraOffset; //offset camera from player
    public float cameraSpeed = 0.1f;

    public Animator animator;

    void Start(){
        transform.position = player.position + cameraOffset;
    }


    void FixedUpdate(){
        Vector3 finalPosition = transform.position;

        //find position to move to
        if (player != null) {
            finalPosition = player.position + cameraOffset;
        }
        
        //move smoothly towards the final position
        Vector3 lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        lerpPosition.z = -15;
        transform.position = lerpPosition;
    }

    //shakes the screen
    public void ScreenShake() {
        animator.SetTrigger("shake");
    }
}
