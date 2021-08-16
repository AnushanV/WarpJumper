using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;

    public int maxHealth = 100;
    int health;

    public HealthBar healthBar;

    public GameObject deathEffect;

    bool canFlinch = true;
    float flinchTimer;
    public float flinchTime;
    public bool hasFlinch = true;
    
    public bool isPortalSpawner = false;
    public bool hasFixedPortalPos = false;
    public GameObject portalPrefab;
    public Vector3 portalOffset = Vector3.zero;
    public Vector3 fixedPortalPos = Vector3.zero;
    Vector3 initPos;
    bool isDead = false;

    public bool spawnsOther = false;
    public GameObject otherObject;

    [SerializeField] AudioSource damagedSound;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;

        health = maxHealth;
        
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(health);

        flinchTimer = flinchTime;
       

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!canFlinch)
        {
            flinchTimer -= Time.deltaTime;
        }

        if (flinchTimer <= 0 && !canFlinch)
        {
            canFlinch = true;
        }

    }

   

    public void takeDamage(int damage) {
        damagedSound.Play();

        health -= damage;
        healthBar.setHealth(health);

        //shake screen if hit
        GameObject.Find("Main Camera").GetComponent<CameraMovement>().ScreenShake();

        if (canFlinch && hasFlinch) {
            canFlinch = false;
            flinchTimer = flinchTime;
            animator.SetTrigger("hurt");
        }

        Debug.Log("Current Health: " + health);

        if (health <= 0) {
            Die();
        }
    }

    private void Die() {

        if (!isDead && spawnsOther)
        {
            Instantiate(otherObject, transform.position, Quaternion.identity);
        }

        if (!isDead) {
            if (isPortalSpawner && !hasFixedPortalPos)
            {
                Instantiate(portalPrefab, initPos + portalOffset, Quaternion.identity);
            }
            else if (isPortalSpawner && hasFixedPortalPos) {
                Instantiate(portalPrefab, fixedPortalPos, Quaternion.identity);
            }
            isDead = true;
        }

        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        animator.SetBool("isDead", true);

        //destroy after a delay in order to let sounds finish playing
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.enabled = false;
        this.enabled = false;
        Destroy(gameObject, 0.2f);
    }

    
}
