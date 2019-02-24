using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEarthWaterSink : MonoBehaviour {

	private AbilityManager abilityManager;
	private void OnEnable()
	{
		Setup();
		
	}
	
	private void OnDisable()
	{
		
	}
	
	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
	}

	
	private void WaterSink()
	{
		
	}
}
