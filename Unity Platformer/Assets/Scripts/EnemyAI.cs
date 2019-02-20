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
    [SerializeField] private EnemyAttack attackOptions;


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
    private PlayerDurability playerHealth;
    private CharacterController2D controller;
    private float attackCountdown;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDurability>();
        controller = playerHealth.GetComponent<CharacterController2D>();
    }

    void FixedUpdate ()
    {

	    Vector3 target = waypoints[currentWaypoint].position;	    
	   
	    
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
	    
        //If enemy is melee...
        if (attackOptions.melee)
        {

            if (attackCountdown <= Time.time && Physics2D.OverlapCircle(transform.position, attackOptions.meleeRange, attackOptions.attackMask))
            {
                
                playerHealth.Hit();
                controller.Knockback(transform.position.x > playerHealth.transform.position.x);
                attackCountdown = Time.time + attackOptions.attackSpeed;

            }

        } else
        //If enemy is ranged...
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(playerHealth.transform.position - transform.position) * attackOptions.range, Color.red);

            if (attackCountdown <= Time.time && Vector2.Distance(transform.position, playerHealth.transform.position) < attackOptions.range)
            {
                GameObject proj = Instantiate(attackOptions.projectile, transform.position, Quaternion.identity);
                Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
                projRb.velocity = Vector3.Normalize(playerHealth.transform.position - transform.position) * attackOptions.projectileSpeed;
                Destroy(proj, 3f);
                attackCountdown = Time.time + attackOptions.attackSpeed;
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
			RaycastHit hit;

			
			if (Physics.Raycast(waypoints[currentWaypoint].position, Vector3.down, out hit, Mathf.Infinity))
			{
				waypointAtGround.y = hit.point.y;
			}
						
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