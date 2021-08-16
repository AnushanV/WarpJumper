using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour{

    [SerializeField] PlayerProgressManager progressManager;

    //return to main menu
    public void menu(){
        SceneManager.LoadScene("MainMenu");
    }

    //close game
    public void exit(){
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Start(){
        GameObject gm = GameObject.FindGameObjectWithTag("GameMaster");

        /*
        if (gm != null) {
            //reset progress after completion
            progressManager.GetComponent<PlayerProgressManager>().save();
        }
        */

    }
}
