using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

	public Transform playerTransform;
	[HideInInspector] public static PlayerAttributes Instance = null;
	public void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
}