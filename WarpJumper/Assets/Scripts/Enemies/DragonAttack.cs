using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{

    public float attackInterval = 4f;
    private float attackTimer;
    public DragonMovement movement;
    public GameObject fireCastAnim;
    public GameObject dragonFireball;
    private void Start()
    {
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.aggro) {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0) {
                StartCoroutine(attack());
                attackTimer = attackInterval;
            }

        }
    }

    IEnumerator attack()
    {
        Instantiate(fireCastAnim, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(dragonFireball, transform.position, Quaternion.identity);
    }
}
