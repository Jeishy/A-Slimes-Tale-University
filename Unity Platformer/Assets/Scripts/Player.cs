﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] private int health;
    [SerializeField] private int armour;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _armourText;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private Element element = Element.None;
    


    private Vector2 damagePoint;                                        // Position where the player was hit by projectile
    private float nextDamageTime;
    private CharacterController2D controller;
    private GameData gm;
    
    private void Start()
    {
        
        
        controller = GetComponent<CharacterController2D>();
        
        gm = GameData.instance;
        
        if (gm.hasData)
        {
            health = gm.health;
            armour = gm.armour;
            transform.position = gm.position;
            element = gm.element;
        }
        
    }

	void Update () {


        //Call Die() function when player is at or below 0 health
        if (health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.O))
            gm.SavePlayer(this);

        if (Input.GetKeyDown(KeyCode.P))
            gm.LoadPlayer(true);
        
        if (Input.GetKeyDown(KeyCode.N))
            Hit();

        if (Input.GetKeyDown(KeyCode.M))
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
        //Load current scene
        gm.LoadPlayer(true);
    }


    //GET FUNCTIONS
    public int GetHealth()
    {
        return health;
    }

    public int GetArmour()
    {
        return armour;
    }

    public int GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public Element GetElement()
    {
        return element;
    }
    
    
    

    //Collision checks
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Check if the player has collided with an enemy projectile
        if (other.gameObject.CompareTag("EnemyProj"))
        {
            //Apply knockback
            controller.Knockback(other.transform.position.x > transform.position.x);
            
            //Destroy projectile
            Destroy(other.gameObject);
            
            //Calculate new health/armour
            Hit();
            
        }

        if (other.gameObject.CompareTag("DeathTrigger"))
        {
            Die();
        }

        if (other.gameObject.CompareTag("NextLevel"))
        {
            Debug.Log("Level complete!!");
            LevelChanger.instance.FadeToLevel(GetCurrentLevel() + 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            gm.SavePlayer(this);
        }


        if (other.CompareTag("NextLevel"))
        {
            Debug.Log("Level complete!!");
            LevelChanger.instance.FadeToLevel(GetCurrentLevel() + 1);
        }

    }
}