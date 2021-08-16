using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;
    public GameObject fireballPrefab;
    public int currentArrow = 0;
    public GameObject player;

    //called from the animation event on the correct animation frame
    public void fireArrow() {

        currentArrow = player.GetComponent<PlayerInput>().currentArrow;

        //create an arrow at the firePoint attached to the player
        switch (currentArrow) {
            case 0:
                Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
                break;
            case 1:
                Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
                break;
            default:
                Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
                break;
        }
        
    }
}
