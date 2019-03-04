using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTarget : MonoBehaviour {

	public GameObject DungeonPanel;
	public GameObject WindPanel;
	public GameObject WaterPanel;
	public GameObject FirePanel;
	public GameObject EarthPanel;

	private Transform dungeonDoor; 
	private Transform windDoor; 
	private Transform waterDoor;
	private Transform fireDoor;
	private Transform earthDoor;
    [SerializeField] private Transform doorLookAt;	

	void Awake()
	{		
		
		dungeonDoor = GameObject.Find("DungeonCentre").transform;		
		windDoor = GameObject.Find("WindCentre").transform;
		waterDoor = GameObject.Find("WaterCentre").transform;
		fireDoor = GameObject.Find("FireCentre").transform;
		earthDoor = GameObject.Find("EarthCentre").transform;

    	
		
	}


	// Update is called once per frame
	void Update() 
	{

		doorLookAt = GameObject.Find("Main Camera").GetComponent<TargetCameraScript>().target;
		DungeonActivate();
		WindActivate();
		WaterActivate();
		FireActivate();
		EarthActivate();
		

		
	}

	void DungeonActivate()
	{
		if (doorLookAt == dungeonDoor)
        {
            DungeonPanel.SetActive(true); //Activates the shop panel UI
		}
		else { DungeonPanel.SetActive(false); }
	}

	void WindActivate()
	{
		if(doorLookAt == windDoor)
			{
				WindPanel.SetActive(true); // Activates the wind panel UI
			}
		else { WindPanel.SetActive(false); }
	}

void WaterActivate()
	{
		if (doorLookAt == waterDoor)
			{
				WaterPanel.SetActive(true); //Activates the water panel UI
			} 
		else { WaterPanel.SetActive(false); }
	}

void FireActivate()
	{
		if(doorLookAt == fireDoor)
			{	
				FirePanel.SetActive(true); //Activates the fire panel UI
			} 
		else { FirePanel.SetActive(false); }
	}

void EarthActivate()
	{
		if (doorLookAt == earthDoor)
			{
				EarthPanel.SetActive(true); //Activates the earth panel UI
			} 
		else { EarthPanel.SetActive(false); }
	}
}
