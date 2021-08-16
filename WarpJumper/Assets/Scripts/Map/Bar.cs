using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{

    public Animator animator;
    bool canKill = false;

    public bool defaultActive = true;

    private void Start()
    {
        if (defaultActive == false) {
            switchActivate();
        }
    }


    public void disableCollision() {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void enableCollision() {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        canKill = true;
    }

    public void resetKill() {
        canKill = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        //kill player if they are inside the bars when they are reappearing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && canKill && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(collision.gameObject.GetComponent<PlayerHealth>().maxHealth);
        }
    }

    public void switchActivate() {
        animator.SetTrigger("switch");
    }
    
}
