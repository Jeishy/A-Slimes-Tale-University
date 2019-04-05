using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{

	[SerializeField] private bool DEBUG = false;                            // Constant used to display debug information

	[Space] [Header("Enemy Options")] 
	[SerializeField] private bool flying;                                   // If true enemy will fly to waypoints
	[SerializeField] private Animator animator;                             // Animator reference
    [SerializeField] private EnemyDurability enemyDurability;               // Serialized class that holds enemy durabilty information (currently only health)
    [SerializeField] private EnemyAttack attackOptions;                     // Serialized class storing attack options (damage, attack speed, projectile, attack style)


    [Header("Patrol Options")]
	[SerializeField] private bool patrol;                                   // If true enemy will patrol between given waypoints
	[SerializeField] private WaypointFollowStyle waypointFollowStyle;       // Waypoint following style (Random waypoint,       Circular: A -> B -> -> C -> A -> B -> C,      Ping-Pong: A -> B -> C -> B-> A)
	[SerializeField] private float patrolSpeed;                             // Waypoint following speed
	
	[Space]
	[SerializeField] private Transform[] waypoints;                         //Waypoints tranform array
	
	[Space] [Header("Waypoint Options")]
    [SerializeField] private bool waitAtWaypoint;				            // If true, enemy will wait a set time at each waypoint
	[SerializeField] private float waitTime;					            // Set wait time enemy waits at the waypoint
    [SerializeField] private bool randomWaitTime;				            // If true, the wait time will be random between minimumRandomWaitTime and maximiumRandomWaitTime
    [SerializeField] private float maximiumRandomWaitTime;		            // Maximum random wait time
    [SerializeField] private float minimumRandomWaitTime;		            // Minimum random wait time
    [SerializeField] private float waypointDetectDistance;                  // How close the enemy needs to be to the waypoint to cnonsider as reached
    [Space]

    private int currentWaypoint = 0;							            // Stores the value of the current waypoint index
    private int waypointIncrement = 1;							            // Used for the Ping-Pong waypoint following method
    private bool waiting = false;								            // When true the enemy pauses its patrol
    private GameObject player;                                              // Stores player game object reference
    private Player playerScript;                                            // Stores player script (holds health, armor etc..)
    private CharacterController2D controller;                               // Stores Character Controller script
    private float attackCountdown;                                          // Attack countdown timer
    private bool m_FacingRight = false;                                     // Flag that tells which way the player is facing
    private AbilityManager _abilityManager;                                 // Reference to the ability manager
    private AudioManager _audioManager;                                     // Reference to the audio manager

    public ElementalStates Element;

    private void Start()
    {
        //Fill references
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        controller = player.GetComponent<CharacterController2D>();
        _audioManager = AudioManager.instance;
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();


    }

    void Update ()
    {

        //If health < 0, you dead :/
        if (enemyDurability.health <= 0)
        {
            Die();
        }

        //Execute code when patrol boolean is true - Set in editor
        if (patrol && !waiting)
	    {
            Patrol();
	    }

        //If enemy is melee...
        if (attackOptions.attackStyle == AttackStyle.Melee)
        {
            MeleeAttack();
	        
        }
        else if (attackOptions.attackStyle == AttackStyle.Ranged)
        //If enemy is ranged...
        {
            RangeAttack();
        }
        //If the enemy is a GHOST!
        else if (attackOptions.attackStyle == AttackStyle.Ghost)
        {
            GhostAttack();
        }



    }

    void Patrol()
    {
	    
        //When enemy has reached its current waypoint, go to next waypoint
        if (IsAtWaypoint())
        {
            NextWaypoint();   
        }
        //Or else keep moving to current waypoint
        else
        {
			
            //Direction to the active waypoint
            Vector2 dir = (waypoints[currentWaypoint].position - transform.position).normalized;

            //Flip the enemy correctly towards the waypoint
            if (dir.x < 0 && m_FacingRight)
                Flip();
            else if (dir.x > 0 && !m_FacingRight)
                Flip();

            //Move the player towards the waypoint at the pace of the 'patrolSpeed'
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position,
                patrolSpeed * Time.deltaTime);
        }
    }
    
    //Melee attack logic
    void MeleeAttack()
    {
        //Check if enemy's attack is off cooldown and it is within melee range of the player
        if (CanAttack() && (Physics.OverlapSphere(transform.position, attackOptions.meleeRange, attackOptions.playerMask).Length > 0))
        {

            //Stop the enemy
	        patrol = false;
	        
            //Play attack animation
	        animator.SetTrigger("Attack");

            //Apply damage synchronised with the attack animation
	        StartCoroutine(DelayedDamage());

            //Calculate next attack time
            attackCountdown = Time.time + attackOptions.attackSpeed;

            //Special ghost attack - After attacking, the ghost disappears and returns to a random waypoint
            if (attackOptions.attackStyle == AttackStyle.Ghost) {
                transform.position = GetRandomWaypoint().position;
                patrol = true;
            }

        }
    }
    
    //Synchronises applied damage with the attack animation
    IEnumerator DelayedDamage() 
    {
        //Wait for X seconds
		yield return new WaitForSeconds(attackOptions.animationDamageDelay);
		
		//Call the Hit() method on the PlayerDurability script
		playerScript.Hit(_abilityManager.CurrentPlayerElementalState, Element);

		//Call the Knockback(bool: right) method on the CharacterController2D script
		controller.Knockback(transform.position.x > player.transform.position.x);

		waiting = false;

    }

    //Range attack logic
    void RangeAttack()
    {
        //If enemy has rotating parts...
        if (attackOptions.rotate)
            //Rotate!
            PointToPlayer(attackOptions.rotator);

        //Get the direction vector towards the player
        Vector2 directionToPlayer = player.transform.position - transform.position;
        RaycastHit hit;


        //Check if any object is not in between the enemy position and the player position, a.k.a if the line of fire is clear


        //Make sure the enemy attack is available and check if the raycast has hit anything
        if (CanAttack() && Physics.Raycast(transform.position, directionToPlayer, out hit, attackOptions.range, attackOptions.rangeAttackMask))
        {
            //Check if the raycast hit the player (therefore it hasn't hit anything in between, so line of fire is clear)
            if (hit.collider.CompareTag("Player"))
            {
                
                //Play shooting animation
                animator.SetTrigger("Shoot");

                //Play audio
                _audioManager.Play("CannonFire");

                //Instantiate the projectile prefab
                GameObject proj = Instantiate(attackOptions.projectile, attackOptions.firePoint.position, Quaternion.identity);

                //Instantiate the particle effect prefab
                GameObject particle = Instantiate(attackOptions.particleEffect, attackOptions.firePoint.position, Quaternion.identity);

                //Access the projectile script
                EnemyProjectile projScript = proj.GetComponent<EnemyProjectile>();

                //Set the correct projectile element (based of the origin enemy)
                projScript.SetElement(Element);

                //Get projectile's rigidbody
                Rigidbody projRb = proj.GetComponent<Rigidbody>();


                //Apply force to projectile's rigidbody
                projRb.velocity = Vector3.Normalize(player.transform.position - transform.position) *
                                  attackOptions.projectileSpeed;

                //Destroy projectile and particle effect after their maximum lifespan has been reached
                Destroy(proj, attackOptions.projectileLifespan);
                Destroy(particle, attackOptions.particleLifespan);

                //Calculate and store next attack time
                attackCountdown = Time.time + attackOptions.attackSpeed;
            }
        }
    }

    //Ghost attac logic
    void GhostAttack()
    {

        //Get direction to play
        Vector2 directionToPlayer = player.transform.position - transform.position;
        RaycastHit hit;
        

        //Check if is in range
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, attackOptions.range, attackOptions.rangeAttackMask) && CanAttack())
        {
            //Make sure its the player
            if (hit.collider.CompareTag("Player")) 
            {
                //Play audio
                _audioManager.Play("GhostAttack");

                //Disable patrol
                patrol = false;

                //Move towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, attackOptions.ghostMoveAttackSpeed * Time.deltaTime);

                //Attempt to attack
                MeleeAttack();
            }

        //If not in range, keep patrolling...
        } else {
            patrol = true;
        }
    }

    //See if attack is ready
    bool CanAttack()
    {
        return attackCountdown <= Time.time;
    }

    //Executes when enemy is hit
    public void Hit(float amount = 1f)
    {
        enemyDurability.health -= amount;

    }

    //Death logic for enemies
    void Die()
    {
        //Ghost plays a death sound
        if (attackOptions.attackStyle == AttackStyle.Ghost)
            _audioManager.Play("GhostDeath");
        Destroy(gameObject);
    }

    //Flips the enemy 180 degrees
    void Flip()
    {
            // Switch the way the enemy is labelled as facing.
            m_FacingRight = !m_FacingRight;

            Vector3 newScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
            transform.Rotate(new Vector3(0, 180, 0));
    }

    //Increments current waypoint variable, then it makes sure it doesnt go over the amount of set waypoints
    void NextWaypoint()
	{
		
		//If waitAtWaypoint boolean is true
		if (waitAtWaypoint)
		{
			//Call the Wait() method
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

		//Random waypoint following method
		if (waypointFollowStyle == WaypointFollowStyle.Random)
		{
			currentWaypoint = Random.Range(0, waypoints.Length);
		}
		
	}


    //Retrives random waypoint transform, used by the ghost enemy
    Transform GetRandomWaypoint() {
        return waypoints[Random.Range(0, waypoints.Length-1)];
    }


    //Rotates transform towards the player, used by the cannon to position its barrel
    void PointToPlayer(Transform rotator)
    {	    
        //Calculates desired rotation
	    Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);

        //Apply rotation
        rotator.transform.rotation = targetRotation;
    }

    //Is called when waiting between waypoints is enabled
	IEnumerator Wait()
	{
		float seconds;  
		
		//If wait time is random
		if (randomWaitTime)
		{
			//Wait time is chosen based on a random number between a minimum and maximum number
			seconds = Random.Range(minimumRandomWaitTime, maximiumRandomWaitTime);
		}
		else
		{
			//If wait time is not random, set it to a preset value
			seconds = waitTime;
		}
			
		
		//Set waiting to true for 'x' amount of seconds
		waiting = true;

        //Stop the walk animation
		animator.SetBool("Walk", false);
		yield return new WaitForSeconds(seconds);

        //Play the walk animation once done waiting
		animator.SetBool("Walk", true);
		waiting = false;

	}

    //Retrives active waypoint position, if enemy isnt flying, it returns the position at ground level
	Vector2 GetWaypoint()
	{

		Vector2 waypointAtGround = waypoints[currentWaypoint].position;
		
        //If enemy is flying it returns the exact position
		if (flying)
		{

			waypointAtGround =  waypoints[currentWaypoint].position;

		}
        //If he doesnt fly, it returns the waypoint at ground level by raycasting straight down
		else
		{
			RaycastHit hit;

			
			if (Physics.Raycast(waypoints[currentWaypoint].position, Vector3.down, out hit, Mathf.Infinity))
			{
				waypointAtGround.y = hit.point.y;
			}
						
		}
		
        //Returns the calculated waypoint position
		return waypointAtGround;
	}
	
    //Checks if the enemy has reached a waypoint
	bool IsAtWaypoint()
	{
        //Gets the active waypoint position
		Vector2 waypoint = GetWaypoint();

        //Return true if within a margin of error
		return (Vector2.Distance(transform.position, waypoint) < waypointDetectDistance);
		
	}
}

//Waypoint following style enum
public enum WaypointFollowStyle
{
	Circular, PingPong, Random
}