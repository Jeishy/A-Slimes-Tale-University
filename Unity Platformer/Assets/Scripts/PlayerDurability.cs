﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDurability : MonoBehaviour {

    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _armourText;
    [SerializeField] private float damageCooldown = 0.5f;
    


    private Vector2 damagePoint;                                        // Position where the player was hit by projectile
    private float nextDamageTime;
    private CharacterController2D controller;
    
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

	void Update () {


        //Call Die() function when player is at or below 0 health
        if (health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.O))
            Hit(1);

        if (Input.GetKeyDown(KeyCode.P))
            armour++;

        //Display health and armour in UI
        _healthText.text = "Health: " + health;
        _armourText.text = "Armour: " + armour;
	}


    public void Hit(int damage = 1)
    {
        if (nextDamageTime <= Time.time)
        {
            //Check if player has armour
            if (armour > 0)
            {
                //If so, remove armour slot
                RemoveArmourSlot();
            }
            else
            {
                //Oterwise, decrement health by 1
                health -= damage;
            }
            

            nextDamageTime = Time.time + damageCooldown;

        }


    }

    
    
    void RemoveArmourSlot()
    {
        //1 hit = 3 armor, if player doesnt have full armour, a single hit cannot damage armour below 3 pieces
        if (armour > 3)
        {
            armour = 3;
        } else
        {
            armour = 0;
        }

    }

    void Die()
    {
        //Debug.LogWarning("Player Died!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.CompareTag("EnemyProj"))
            {
                controller.Knockback(other.transform.position.x > transform.position.x);
                Destroy(other.gameObject);
                Hit();
                
            }
        }
    }
}
