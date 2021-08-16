using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public Animator animator;

    private void Awake()
    {
        Debug.Log("MusicManager Awake");
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else {
            Destroy(gameObject);
        }
        */
    }

    public void musicDone() {
        Destroy(gameObject);
    }
}
