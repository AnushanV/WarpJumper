using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour{

    public int maxHealth = 200;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject levelLoader;

    [SerializeField] AudioSource damageSound;

    void Start(){
        levelLoader = GameObject.Find("LevelLoader");
        //set current health as the max health
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    public void takeDamage(int damage){
        damageSound.Play(); //play sound effect

        //decrease player health by damage
        currentHealth -= damage;
        Debug.Log("PLAYER: " + currentHealth);
        healthBar.setHealth(currentHealth); //adjust healthbar slider

        //create particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("TakeHit"), transform.position, transform.rotation) as GameObject;
        Destroy(particles, 0.5f);

        //kill player and reload scene when health goes below 0
        if (currentHealth <= 0) {
            int respawnScene = SceneManager.GetActiveScene().buildIndex;
            levelLoader.GetComponent<Fade>().LoadNextScene(respawnScene);
            GameObject.Destroy(gameObject);
        }
    }
}
