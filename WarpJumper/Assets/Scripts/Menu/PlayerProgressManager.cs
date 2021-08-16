using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour{
    public PlayerProgress playerProgress = null;
    private string savePath;

    private void Awake(){
        savePath = Application.persistentDataPath + "/progress.json";

        //create a new player progress object
        this.playerProgress = new PlayerProgress();
    }

    [ContextMenu("Save")]
    public void save() {
        //save the current player progress
        LoaderSaver.SavePlayerProgressAsJSON(savePath, this.playerProgress);
    }

    [ContextMenu("Load")]
    public void load(){
        //load the data into player progress
        this.playerProgress = LoaderSaver.LoadPlayerProgressFromJSON(savePath);
    }
}
