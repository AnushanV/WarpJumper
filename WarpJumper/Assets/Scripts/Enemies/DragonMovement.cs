using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    [SerializeField] LayerMask players;

    //movement variables
    [SerializeField] float movementSpeed = 1f;
    public int direction;
    
    public bool aggro = false;
    GameObject player;

    //where to be relative to player
    public float hoverX = 2f;
    public float hoverY = 4f;
    public Vector3 destination = Vector3.zero;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (aggro && player != null) {

            //find position of player
            float xDiff = transform.position.x - player.transform.position.x;
            float yDiff = transform.position.y - player.transform.position.y;

            //flip sprite
            if (xDiff <= 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                destination = player.transform.position - new Vector3(hoverX, 0f, 0f) + new Vector3(0f, hoverY, 0f);
            }
            else {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                destination = player.transform.position + new Vector3(hoverX, 0f, 0f) + new Vector3(0f, hoverY, 0f);

            }

            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);


        }
        
    }
}
