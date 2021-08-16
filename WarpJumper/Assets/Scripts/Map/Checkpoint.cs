using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour{

    private GameMaster gm;
    private bool activated = false;
    public Animator animator;
    [SerializeField] AudioSource shineSound;

    private void Start(){
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D other){

        //set the game master's checkpoint position after player collides with checkpoint
        if (other.CompareTag("Player")) {
            if (!activated) {
                shineSound.Play();
                activated = true;
                animator.SetTrigger("activate");
                gm.lastCheckpointPos = transform.position;
            }
        }
    }

    //destroy the checkpoint
    public void destroy() {
        Destroy(gameObject);
    }
}
