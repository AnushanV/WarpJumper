using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour{

    public Animator animator;
    public float fadeDuration;

    public void LoadNextScene(int sceneIndex) {
        StartCoroutine(LoadLevel(sceneIndex)); //load next level
    }

    IEnumerator LoadLevel(int sceneIndex) {
        //show a fade animation
        animator.SetTrigger("startFade");
        

        //load next scene after a delay
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneIndex);
    }
         
}
