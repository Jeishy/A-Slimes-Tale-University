using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{

	[SerializeField] private bool DEBUG = false;

	[Space] [Header("Enemy Options")] 
	[SerializeField] private bool flying;
	[SerializeField] private Animator animator;
    [SerializeField] private EnemyDurability enemyDurability;
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
    [SerializeField] private float waypointDetectDistance;      //How close the enemy needs to be to the waypoint to cnonsider as reached
    [Space]

    private int currentWaypoint = 0;							//Stores the value of the current waypoint index
    private int waypointIncrement = 1;							//Used for the Ping-Pong waypoint following method
    private bool waiting = false;								//When true the enemy pauses its patrol
    private GameObject player;
    private Player playerScript;
    private CharacterController2D controller;
    private float attackCountdown;
    private bool m_FacingRight = false;
    private AbilityManager _abilityManager;
    private AudioManager _audioManager;
    private bool ghostAttacked = false;

    public ElementalStates Element;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        controller = player.GetComponent<CharacterController2D>();
        _audioManager = AudioManager.instance;
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();


    }

    void Update ()
    {

        if (enemyDurability.health <= 0)
        {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.V))
            Die();

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
			
            Vector2 dir = (waypoints[currentWaypoint].position - transform.position).normalized;

            if (dir.x < 0 && m_FacingRight)
                Flip();
            else if (dir.x > 0 && !m_FacingRight)
                Flip();

            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position,
                patrolSpeed * Time.deltaTime);
        }
    }

    void BeginMelee()
    {
	    patrol = false;
    }

    void EndMelee()
    {
	    patrol = true;
    }
    
    void MeleeAttack()
    {
        //Check if enemy's attack is off cooldown and it is within melee range of the player
        if (CanAttack() && (Physics.OverlapSphere(transform.position, attackOptions.meleeRange, attackOptions.playerMask).Length > 0))
        {
	        waiting = true;
	        
	        animator.SetTrigger("Attack");

	        StartCoroutine(DelayedDamage());

            //Calculate next attack time
            attackCountdown = Time.time + attackOptions.attackSpeed;

            if (attackOptions.attackStyle == AttackStyle.Ghost) {
                transform.position = GetRandomWaypoint().position;
                patrol = true;
            }

        }
    }
    
    IEnumerator DelayedDamage() 
    {
		yield return new WaitForSeconds(attackOptions.animationDamageDelay);
		
		//Call the Hit() method on the PlayerDurability script
		playerScript.Hit(_abilityManager.CurrentPlayerElementalState, Element);

		//Call the Knockback(bool: right) method on the CharacterController2D script
		controller.Knockback(transform.position.x > player.transform.position.x);

		waiting = false;

    }

    void RangeAttack()
    {
        if (attackOptions.rotate)
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
                Debug.Log("Firing projectile at player");
                //Instantiate the projectile prefab
                animator.SetTrigger("Shoot");
                _audioManager.Play("CannonFire");
                GameObject proj = Instantiate(attackOptions.projectile, attackOptions.firePoint.position, Quaternion.identity);
                GameObject particle = Instantiate(attackOptions.particleEffect, attackOptions.firePoint.position, Quaternion.identity);

                EnemyProjectile projScript = proj.GetComponent<EnemyProjectile>();
                projScript.SetElement(Element);

                //Get projectile's rigidbody
                Rigidbody projRb = proj.GetComponent<Rigidbody>();


                //Apply force to projectile's rigidbody
                projRb.velocity = Vector3.Normalize(player.transform.position - transform.position) *
                                  attackOptions.projectileSpeed;

                //Destroy projectile after it's maximum lifespan has been reached
                Destroy(proj, attackOptions.projectileLifespan);
                Destroy(particle, attackOptions.particleLifespan);

                //Calculate and store next attack time
                attackCountdown = Time.time + attackOptions.attackSpeed;
            }
        }
    }

    void GhostAttack()
    {

        Vector2 directionToPlayer = player.transform.position - transform.position;
        RaycastHit hit;
        

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, attackOptions.range, attackOptions.rangeAttackMask) && CanAttack())
        {
            if (hit.collider.CompareTag("Player")) 
            {
                _audioManager.Play("GhostAttack");
                patrol = false;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, attackOptions.ghostMoveAttackSpeed * Time.deltaTime);
                MeleeAttack();
            }

        } else {
            patrol = true;
        }
    }

    bool CanAttack()
    {
        return attackCountdown <= Time.time;
    }

    public void Hit(float amount = 1f)
    {
        enemyDurability.health -= amount;

    }

    void Die()
    {
        if (attackOptions.attackStyle == AttackStyle.Ghost)
            _audioManager.Play("GhostDeath");
        Destroy(gameObject);
    }

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


    Transform GetRandomWaypoint() {
        return waypoints[Random.Range(0, waypoints.Length-1)];
    }

    void PointToPlayer(Transform rotator)
    {	    
	    Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
	    rotator.transform.rotation = targetRotation;
        //rotator.transform.LookAt(player.transform);
    }

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
		animator.SetBool("Walk", false);
        Debug.Log("Stopped walking");
		yield return new WaitForSeconds(seconds);
		animator.SetBool("Walk", true);
        Debug.Log("Started walking");
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