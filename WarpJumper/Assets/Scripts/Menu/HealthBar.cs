using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void setMaxHealth(int maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void setHealth(int health) {
        slider.value = health;
    }
}
