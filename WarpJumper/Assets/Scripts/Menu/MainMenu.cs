using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] PlayerProgressManager progressManager;
    public GameMaster gm;
    PlayerProgress playerProgress;
    
    //start the game
    public void play() {

        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        string savePath = Application.persistentDataPath + "/progress.json";

        //check if there is no save file and create a default save
        if (!File.Exists(savePath)){
            PlayerProgress newSave = new PlayerProgress();

            string json = JsonUtility.ToJson(newSave);
            File.WriteAllText(savePath, json);
        }

        //load the save data
        progressManager.load();
        playerProgress = progressManager.GetComponent<PlayerProgressManager>().playerProgress;

        int levelNum = playerProgress.levelNum;
        float x = playerProgress.lastCheckpointPos[0];
        float y = playerProgress.lastCheckpointPos[1];
        
        //set the spawn point for player
        gm.lastCheckpointPos = new Vector2(x, y);
        
        //load the saved scene
        SceneManager.LoadScene(levelNum);
    }

    //show the more menu
    public void more(){
        SceneManager.LoadScene("MoreMenu");
    }

    //close the game
    public void exit(){
        Debug.Log("Exit");
        Application.Quit();
    }
}
