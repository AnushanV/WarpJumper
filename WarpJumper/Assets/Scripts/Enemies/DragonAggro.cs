using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAggro : MonoBehaviour
{

    public DragonMovement dragonMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            dragonMovement.aggro = true;
        }
    }
}
