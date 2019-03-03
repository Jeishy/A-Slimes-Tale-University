using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The 5 different states the player can be in
// numbers assigned to states can be used for 
// calculations if needed
public enum ElementalStates
{
	None = 0, Fire = 1, Water = 2, Wind = 3, Earth = 4
}

public class AbilityManager : MonoBehaviour {

	#region Delegates and Events
	public delegate void AbilityEventHandler();
	// This event is for debugging, allows player to change
	// their state by pressing Q
	public event AbilityEventHandler OnPlayerSwitchAbility;
	public event AbilityEventHandler OnProjectileFire;
	public event AbilityEventHandler OnBoostedProjectileFire;
	public event AbilityEventHandler OnWindState;
	public event AbilityEventHandler OnFireState;
	public event AbilityEventHandler OnWaterState;
	public event AbilityEventHandler OnEarthState;

	public event AbilityEventHandler OnEarthCrash;
    #endregion

    [HideInInspector] public bool IsAimToShoot;
    [HideInInspector] public float InitialGravityScale;
    [HideInInspector] public Rigidbody2D playerRb;

	// Holds the elemental state of the player,
	// uses accessors to encapsulate current elemental state
	private ElementalStates _currentPlayerElementalState;
	public ElementalStates CurrentPlayerElementalState
	{
		get { return _currentPlayerElementalState;}
		set { _currentPlayerElementalState = value;}
	}

	private void Start()
	{
		// Set elemental state to None at beginning of the game
		CurrentPlayerElementalState = ElementalStates.None;
        InitialGravityScale = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        IsAimToShoot = true;
	}

	// Method for running methods subscribed to OnPlayerSwitchAbility event
	// Currently used for debugging purposes only
	public void PlayerSwitchAbility()
	{
		if (OnPlayerSwitchAbility != null)
		{
			OnPlayerSwitchAbility();
		}
	}

	// Method for running methods subscribed to OnProjectileFire event
	// Implementation in PlayerProjectile class
	// Triggered in AbilityInputHandler class
	public void ProjectileFire()
	{
		if (OnProjectileFire != null)
		{
			OnProjectileFire();
		}
	}

	public void BoostedProjectileFire()
	{
		if (OnBoostedProjectileFire != null)
		{
			OnBoostedProjectileFire();
		}
	}

	// Method for running methods subscribed to OnWindState event
	// All wind state methods are run via this function
	public void WindState()
	{
		if (OnWindState != null)
		{
			OnWindState();
		}
	}

	// Method for running methods subscribed to OnFireState event
	// All fire state methods are run via this function
	public void FireState()
	{
		if (OnFireState != null)
		{
			OnFireState();
		}
	}

	// Method for running methods subscribed to OnWaterState event
	// All water state methods are run via this function
	public void WaterState()
	{
		if (OnWaterState != null)
		{
			OnWaterState();
		}
	}

	// Method for running methods subscribed to OnEarthState event
	// All earth state methods are run via this function
	public void EarthState()
	{
		if (OnEarthState != null)
		{
			OnEarthState();
		}
	}

    // Method for running methods subscribed to OnEarthCrash event
    // This is an active ability available to the player when they enter the earth state
    public void EarthCrash()
	{
		if (OnEarthCrash != null)
		{
			OnEarthCrash();
		}
	}
}
