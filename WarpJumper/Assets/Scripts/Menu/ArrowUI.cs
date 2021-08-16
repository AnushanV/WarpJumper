using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{

    public PlayerInput player;
    public Image arrow;
    public Image fireball;

    //update the ui icon to match the player's current arrow
    private void LateUpdate()
    {
        switch (player.currentArrow) {
            case 0:
                arrow.enabled = true;
                fireball.enabled = false;
                break;
            case 1:
                fireball.enabled = true;
                arrow.enabled = false;
                break;
            default:
                arrow.enabled = true;
                fireball.enabled = true;
                break;
        }
    }
}
