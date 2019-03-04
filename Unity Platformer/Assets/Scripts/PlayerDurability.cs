using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDurability : MonoBehaviour {

    [SerializeField] int health;

    // Made armour variable public to allow it
    // to be changed outside of script
    public int armour;

    [SerializeField] Text _healthText;
    [SerializeField] Text _armourText;
    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {


        //Call Die() function when player is at or below 0 health
        if (health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.O))
            Hit();

        if (Input.GetKeyDown(KeyCode.P))
            armour++;

        //Display health and armour in UI
        _healthText.text = "Health: " + health;
        _armourText.text = "Armour: " + armour;
    }


    void Hit()
    {
        //Check if player has armour
        if (armour > 0)
        {
            //If so, remove armour slot
            RemoveArmourSlot();
        } else
        {
            //Oterwise, decrement health by 1
            health--;
        }


    }

    public void RemoveArmourSlot()
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
        Debug.LogWarning("Player Died!");
    }

}
