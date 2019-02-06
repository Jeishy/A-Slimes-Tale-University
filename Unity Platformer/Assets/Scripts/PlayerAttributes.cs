using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

	public Transform playerTransform;

	// Singleton pattern for accessing class
	[HideInInspector] public static PlayerAttributes Instance = null;
	public void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		// Caching player transform
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
}