using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public GameObject[] connected; //object connected to the switch
    public Animator animator;
    public bool on = false;
    public bool destroyOnToggle = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Switch hit by: " + other.gameObject.name);
        animator.SetTrigger("switch");
        on = !on;

        foreach (GameObject bar in connected) {
            //destroy dragons
            if (bar != null && bar.CompareTag("Dragon"))
            {
                Destroy(bar);
            }
            //toggle bars
            else if(bar != null) {
                bar.GetComponent<Bar>().switchActivate();
            }
        }

        if (destroyOnToggle) {
            Destroy(gameObject);
        }
    }
}
