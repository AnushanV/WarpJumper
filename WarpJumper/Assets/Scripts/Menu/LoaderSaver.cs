using UnityEngine;
using System.IO;

public class LoaderSaver : MonoBehaviour{

    //saves the player progress
    public static void SavePlayerProgressAsJSON(string savePath, PlayerProgress progress) {
        
        //convert progress to json
        string json = JsonUtility.ToJson(progress);
        File.WriteAllText(savePath, json);
    }

    //loads the player progress
    public static PlayerProgress LoadPlayerProgressFromJSON(string savePath) {

        //load from the file path
        if (File.Exists(savePath)){

            //return the progress object obtained from save file
            string json = File.ReadAllText(savePath);
            PlayerProgress progress = JsonUtility.FromJson<PlayerProgress>(json);
            return progress;
        }
        else {
            Debug.LogError("Unable to load file: " + savePath);
        }

        return null;
    }
}
