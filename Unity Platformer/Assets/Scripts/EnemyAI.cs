using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{

	[SerializeField] private bool DEBUG = false;

	[Space] [Header("Enemy Options")] 
	[SerializeField] private bool flying;
	[SerializeField] private bool melee;
	[SerializeField] private float range;
	
	[Header("Patrol Options")]
	[SerializeField] private bool patrol;
	[SerializeField] private WaypointFollowStyle waypointFollowStyle;
	[SerializeField] private float patrolSpeed; 
	
	[Space]
	[SerializeField] private Transform[] waypoints;
	private Vector2[] waypointsAtStart;
	
	[Space] [Header("Waypoint Options")]
    [SerializeField] private bool waitAtWaypoint;				//If true, enemy will wait a set time at each waypoint
	[SerializeField] private float waitTime;					//Set wait time enemy waits at the waypoint
    [SerializeField] private bool randomWaitTime;				//If true, the wait time will be random between minimumRandomWaitTime and maximiumRandomWaitTime
    [SerializeField] private float maximiumRandomWaitTime;		//Maximum random wait time
    [SerializeField] private float minimumRandomWaitTime;		//Minimum random wait time
    [SerializeField] private float waypointDetectDistance;		//How close the enemy needs to be to the waypoint to cnonsider as reached
   
    private int currentWaypoint = 0;							//Stores the value of the current waypoint index
    private int waypointIncrement = 1;							//Used for the Ping-Pong waypoint following method
    private bool waiting = false;								//When true the enemy pauses its patrol


    void Update ()
    {


	    Vector3 target = waypoints[currentWaypoint].position;
	    //target.y = transform.position.y;
	    
	    
	    
	    //Execute code when patrol boolean is true - Set in editor
	    if (patrol && !waiting)
	    {
		    //When enemy has reached its current waypoint, go to next waypoint
		    if (IsAtWaypoint())
		    {
			    NextWaypoint();
		    }
		    //Or else keep moving to current waypoint
		    else
		    {
			    transform.position = Vector2.MoveTowards(transform.position, GetWaypoint(),
				    patrolSpeed * Time.deltaTime);
		    }
	    }
	    
	    
    }



	//Increments current waypoint variable, then it makes sure it doesnt go over the amount of set waypoints
	void NextWaypoint()
	{
		
		if (waitAtWaypoint)
		{
			StartCoroutine(Wait());
		}
		

		//Circular waypoitn following method: A -> B -> C -> A -> B -> C
		if (waypointFollowStyle == WaypointFollowStyle.Circular)
		{
			currentWaypoint++;
			if (currentWaypoint + 1 > waypoints.Length)
			{			
				currentWaypoint = 0;
			}
		}

		//Ping-Pong waypoint following method: A -> B -> C -> B -> A
		if (waypointFollowStyle == WaypointFollowStyle.PingPong)
		{
			if (currentWaypoint == 0)
				waypointIncrement = 1;
			
			if (currentWaypoint + 2 > waypoints.Length)
			{			
				waypointIncrement = -1;
			}
			
			currentWaypoint += waypointIncrement;
		}

		if (waypointFollowStyle == WaypointFollowStyle.Random)
		{
			currentWaypoint = Random.Range(0, waypoints.Length);
		}
		
		
		Debug.Log("Going to next waypoint (" + (currentWaypoint+1) + "/" + waypoints.Length + ")");
	}

	IEnumerator Wait()
	{
		float seconds;
		
		if (randomWaitTime)
		{
			seconds = Random.Range(minimumRandomWaitTime, maximiumRandomWaitTime);
		}
		else
		{
			seconds = waitTime;
		}
		
		Debug.Log("Waiting at waypoint '" + (currentWaypoint + 1) + "' for " + seconds + " seconds...");

		
		
		waiting = true;
		yield return new WaitForSeconds(seconds);
		waiting = false;

	}

	Vector2 GetWaypoint()
	{


		Vector2 waypointAtGround = waypoints[currentWaypoint].position;
		
		if (flying)
		{

			waypointAtGround =  waypoints[currentWaypoint].position;

		}
		else
		{
			Debug.Log("Not Flying");
			RaycastHit hit;

			
			if (Physics.Raycast(waypoints[currentWaypoint].position, Vector3.down, out hit, Mathf.Infinity))
			{
				waypointAtGround.y = hit.point.y;
				Debug.Log("New waypoint: " + waypointAtGround.y);
			}
			
			Debug.DrawLine(waypoints[currentWaypoint].position, hit.point, Color.red);

			
		}
		
		return waypointAtGround;
	}
	
	bool IsAtWaypoint()
	{

		Vector2 waypoint = GetWaypoint();
		return (Vector2.Distance(transform.position, waypoint) < waypointDetectDistance);
		
	}
}

public enum WaypointFollowStyle
{
	Circular, PingPong, Random
}