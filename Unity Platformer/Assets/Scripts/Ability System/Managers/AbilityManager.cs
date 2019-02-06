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
	public event AbilityEventHandler OnWindState;
	#endregion

    [HideInInspector] public bool IsAimToShoot;

	// Holds the elemental state of the player,
	// uses accessors to encapsulate current elemental state
	private ElementalStates currentPlayerElementalState;
	public ElementalStates CurrentPlayerElementalState
	{
		get { return currentPlayerElementalState;}
		set { currentPlayerElementalState = value;}
	}

	private void Start()
	{
		// Set elemental state to None at beginning of the game
		CurrentPlayerElementalState = ElementalStates.None;
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

	// Method for running methods subscribed to OnWindState event
	// All wind state methods are run via this function
	public void WindState()
	{
		if (OnWindState != null)
		{
			OnWindState();
		}
	}
}
