using UnityEngine;
using UnityEngine.SceneManagement;

public class MoreMenu : MonoBehaviour
{
    [SerializeField] PlayerProgressManager progressManager;
    PlayerProgress playerProgress;

    //overwrite the save data with the default save
    public void clearSaveData(){
        Debug.Log("Clear save data");

        //replace progress with default parameters
        PlayerProgress newPlayerProgress = new PlayerProgress();
        this.progressManager.playerProgress = newPlayerProgress;
        this.progressManager.save();
    }

    //return to main menu
    public void back(){
        SceneManager.LoadScene("MainMenu");
    }
}
