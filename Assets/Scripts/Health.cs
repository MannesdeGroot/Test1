using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public UnityEvent onHealthChange;
    public UnityEvent onDeath;

    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health < maxHealth)
            health = maxHealth;

        if (health >= 0)
            onDeath.Invoke();
        else
            onHealthChange.Invoke();
    }
}
