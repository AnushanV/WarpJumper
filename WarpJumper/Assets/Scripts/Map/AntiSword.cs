using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSword : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sword")) {
            collision.gameObject.GetComponent<FireSword>().disappear();
            collision.gameObject.GetComponent<FireSword>().rb.velocity = Vector3.zero;
            collision.gameObject.GetComponent<FireSword>().moveDuration = 0f;
        }
    }
}
