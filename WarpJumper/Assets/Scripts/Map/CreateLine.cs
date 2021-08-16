using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour{
    
    [SerializeField] int damage = 10;

    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer;
    
    Vector3 initialPlayerPos;
    LineRenderer line;

    bool particleInstantiated = false;

    bool startHitDetection = false;
    Vector2 direction;
    Vector3 lineOffset;

    public float hitDetectionLimit = 0.1f; //size to start hit detection
    public float initialLineWidth = 0.3f; //starting width
    public float lineDuration = 1.1f; //duration of the line
    public float shrinkRate = 0.3f; //speed that the line shrinks

    void Start(){

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null) {
            GameObject lineGenerator = GameObject.FindGameObjectWithTag("LineGenerator");

            //pick a position
            lineOffset = lineGenerator.GetComponent<GenerateLines>().lineOffset;
            initialPlayerPos = player.position + lineOffset;

            //calculate direction to player from the picked position
            float rise = initialPlayerPos.y - transform.position.y;
            float run = initialPlayerPos.x - transform.position.x;

            direction = new Vector2(run, rise).normalized;

            //draw the line
            line = gameObject.GetComponent<LineRenderer>();

            line.startWidth = initialLineWidth;
            line.endWidth = initialLineWidth;

            //create line passing player and this object
            line.SetPosition(0, transform.position - new Vector3(direction.x, direction.y) * 15);
            line.SetPosition(1, initialPlayerPos + new Vector3(direction.x, direction.y) * 15);
        }

        Destroy(gameObject, lineDuration);
    }

    void Update(){

        if (line != null) {

            //shrink the line based on shrink rate
            line.startWidth -= shrinkRate * Time.deltaTime;
            line.endWidth -= shrinkRate * Time.deltaTime;

            //start hit detection when the line becomes small enough
            if (line.startWidth <= hitDetectionLimit && !particleInstantiated){
                startHitDetection = true;
                particleInstantiated = true;

                //calculate the rotation for the particle effect
                float rise = initialPlayerPos.y - transform.position.y;
                float run = initialPlayerPos.x - transform.position.x;

                float angle = Mathf.Atan(rise / run) * 180 / Mathf.PI;

                //create particle effect
                GameObject particles = GameObject.Instantiate(Resources.Load("LineEffect"), transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
                Destroy(particles, 1f);
            }

            //apply damage and knockback to player if they touch the line
            if (startHitDetection){
                
                //hitbox line
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 30, playerLayer);

                //apply damage and knockback
                if (hit.collider){
                    startHitDetection = false;

                    Collider2D playerCollider = hit.collider;
                    //apply X knockback
                    playerCollider.GetComponent<PlayerInput>().modX = 4 * Mathf.Sign(direction.x);
                    playerCollider.GetComponent<PlayerInput>().modTimer = 0.5f;

                    player.GetComponent<PlayerHealth>().takeDamage(damage);

                    //apply Y knockback
                    playerCollider.attachedRigidbody.velocity = new Vector2(playerCollider.attachedRigidbody.velocity.x, 8f);
                }
            }
        }
        
    }
}
