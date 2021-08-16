using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    //points for platform to travel to
    public Transform startPoint;
    public Transform endPoint;
    public Transform startPos;
    Vector3 nextPos;

    public float movementSpeed;
    public bool isMoving = true;

    void Start(){
        nextPos = startPos.position; //move towards initial position
    }

    void Update(){

        //set the platforms appropriate target position
        if (transform.position == startPoint.position){
            nextPos = endPoint.position;
        }
        else if (transform.position == endPoint.position) {
            nextPos = startPoint.position;
        }

        //move the platform towards target
        if (isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, movementSpeed * Time.deltaTime);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //set player as a child to move them along with platform
        if (collision.gameObject.CompareTag("Player")) {
            isMoving = true;
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        //detach player as child
        if (collision.gameObject.CompareTag("Player")){
            collision.collider.transform.SetParent(null);
        }
    }

    //platform path visualization
    private void OnDrawGizmosSelected(){
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
