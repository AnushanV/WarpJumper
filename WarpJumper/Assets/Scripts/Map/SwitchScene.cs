using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchScene : MonoBehaviour
{
    private GameMaster gm;
    public GameObject levelLoader;

    [SerializeField] List<Vector2> startPositions;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource warpSound;

    //public GameObject track1;
    /*
    public GameObject track2;
    public GameObject track3;
    */
    private void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");

        startPositions = new List<Vector2>();

        startPositions.Add(new Vector2(-6, -1)); //level 1
        startPositions.Add(new Vector2(-6, -1)); //level 2
        startPositions.Add(new Vector2(-6, -1)); //level 3
        startPositions.Add(new Vector2(-8.5f, -3f)); //level 4
        startPositions.Add(new Vector2(-8.5f, -3f)); //level 5
        startPositions.Add(new Vector2(-8.5f, -3f)); //level 6
        startPositions.Add(new Vector2(-8.5f, -3f)); //level 7
        startPositions.Add(new Vector2(0f, -3f)); //level 8
        startPositions.Add(new Vector2(0f, 0f)); //end screen

        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        
        //move player to next level when they enter portal
        if (other.CompareTag("Player")) {
            warpSound.Play();

            animator.SetTrigger("despawn");
            //get current level
            int currentLevel = SceneManager.GetActiveScene().buildIndex;

            //set the next level's spawn point
            gm.lastCheckpointPos = startPositions[currentLevel];

            Destroy(other.gameObject);
            
            //load next level
            levelLoader.GetComponent<Fade>().LoadNextScene(currentLevel + 1);

            //change music
            /*
            if (currentLevel == 3 && track2 != null)
            {
                Debug.Log("CHANGE MUSIC START");
                //destroy other tracks
                GameObject track1 = GameObject.FindGameObjectWithTag("Track1");
                if (track1 != null)
                    track1.GetComponent<MusicManager>().animator.SetTrigger("fadeOut");
                Instantiate(track2);
                Debug.Log("CHANGE MUSIC END");
            }
            else if (currentLevel == 7 && track2 != null)
            {
                //destroy other tracks
                track2.GetComponent<MusicManager>().animator.SetTrigger("fadeOut");
                Instantiate(track2);
            }
            else if (currentLevel == 8 && track3 != null)
            {
                //destroy other tracks
                track2.GetComponent<MusicManager>().animator.SetTrigger("fadeOut");
                Instantiate(track3);
            }
            */
        }
    }
}
