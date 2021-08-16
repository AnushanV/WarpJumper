using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject progressManager;

    void Update(){
        //toggle menu when escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused){
                resume();
            }
            else {
                pause();
            }
        }
    }

    //pause the game
    void pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //stop time advancing
        isPaused = true;
    }

    //resume the game
    public void resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; //continue time advancing
        isPaused = false;
    }

    //save the game
    public void save() {
        //create a progress object with current progress
        GameObject gm = GameObject.FindGameObjectWithTag("GameMaster");
        Vector2 lastCheckpointpos = gm.GetComponent<GameMaster>().lastCheckpointPos;
        PlayerProgress currentProgress = new PlayerProgress(SceneManager.GetActiveScene().buildIndex, lastCheckpointpos.x, lastCheckpointpos.y);

        //send save data to progress manager
        progressManager.GetComponent<PlayerProgressManager>().playerProgress = currentProgress;
        progressManager.GetComponent<PlayerProgressManager>().save();
    }

    //saves the game then exit
    public void saveAndExit() {
        save();
        //unpause and head back to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
