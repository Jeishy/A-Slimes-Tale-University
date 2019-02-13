using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

	[SerializeField] private bool DEBUG = false;

	
	[Header("Patrol Options")]
	[SerializeField] private bool patrol;
	[SerializeField] private WaypointFollowStyle waypointFollowStyle;
	[SerializeField] private float patrolSpeed; 
	
	[Space]
	[SerializeField] private Transform[] waypoints;
	private Vector2[] waypointsAtStart;
	
	[Space]
	[Header("Waypoint Options")]
    [SerializeField] private bool waitAtWaypoint;				//If true, enemy will wait a set time at each waypoint
	[SerializeField] private float waitTime;					//Set wait time enemy waits at the waypoint
    [SerializeField] private bool randomWaitTime;				//If true, the wait time will be random between minimumRandomWaitTime and maximiumRandomWaitTime
    [SerializeField] private float maximiumRandomWaitTime;		//Maximum random wait time
    [SerializeField] private float minimumRandomWaitTime;		//Minimum random wait time
    [SerializeField] private float waypointDetectDistance;		//How close the enemy needs to be to the waypoint to cnonsider as reached
   
    private CharacterController controller;
    private int currentWaypoint = 0;
   
	void Update () {
		
		//Execute code when patrol boolean is true - Set in editor
		if (patrol)
		{
			//When enemy has reached its current waypoint, go to next waypoint
			if (IsAtWaypoint())
			{
				NextWaypoint();
			}
			//Or else keep moving to current waypoint
			else
			{
				transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position,
					patrolSpeed * Time.deltaTime);
			}
		}
	}



	//Increments current waypoint variable, then it makes sure it doesnt go over the amount of set waypoints
	void NextWaypoint()
	{
		
		currentWaypoint++;
		if (currentWaypoint + 1 > waypoints.Length)
		{			
			currentWaypoint = 0;
		}
		
		Debug.Log("Going to next waypoint (" + (currentWaypoint+1) + "/" + waypoints.Length + ")");
	}
	
	bool IsAtWaypoint()
	{
		return (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < waypointDetectDistance);
	}
}

public enum WaypointFollowStyle
{
	Circular, Linear
}