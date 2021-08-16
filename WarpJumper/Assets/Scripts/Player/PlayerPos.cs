using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;

    //stores the position of the player
    void Start(){
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        //store the player's position in the game master
        if(gm != null)
            transform.position = gm.lastCheckpointPos;
    }

    void Update(){

        //restart the level if player presses r
        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
