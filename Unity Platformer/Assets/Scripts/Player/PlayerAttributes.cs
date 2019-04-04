using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

	[HideInInspector] public Transform playerTransform;
    [HideInInspector] public GameObject Player;

	// Singleton pattern for accessing class
	[HideInInspector] public static PlayerAttributes Instance = null;
	public void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}

        Player = GameObject.FindGameObjectWithTag("Player");
        // Caching player transform
        playerTransform = Player.transform;
	}
}