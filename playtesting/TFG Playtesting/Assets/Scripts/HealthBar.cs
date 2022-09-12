﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerMovement player;
    public float currentHealth;
    private float maxHealth = 100f;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player.playerHealth;
        healthBar.value = currentHealth / maxHealth;
    }

}
