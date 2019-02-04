using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmour : MonoBehaviour {

    [SerializeField] int armour;
    
    // Store reference to AbilityManager go
    // Store reference of CurrentPlayerElementalState



    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
        //Set element to 'none' if all armour is lost

        if (armour <= 0)
        {
            //Do stuff
        }
	}
}
