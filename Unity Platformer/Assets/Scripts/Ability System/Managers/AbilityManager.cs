using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalStates
{
	None = 0, Fire = 1, Water = 2, Wind = 3, Earth = 4
}

public class AbilityManager : MonoBehaviour {

	// Delegates and events for ability system events
	public delegate void AbilityEventHandler();
	// This event is for debugging, allows player to change
	// their state by pressing Q
	public event AbilityEventHandler OnPlayerSwitchAbility;
	public event AbilityEventHandler OnProjectileFire;
	public event AbilityEventHandler OnWindState;

	// Events for armour here?

    [HideInInspector] public bool IsAimToShoot;

	// Holds the elemental state of the player
	private ElementalStates currentPlayerElementalState;
	public ElementalStates CurrentPlayerElementalState
	{
		get { return currentPlayerElementalState;}
		set { currentPlayerElementalState = value;}
	}

	private void Start()
	{
		CurrentPlayerElementalState = ElementalStates.None;
	}

	public void PlayerSwitchAbility()
	{
		if (OnPlayerSwitchAbility != null)
		{
			OnPlayerSwitchAbility();
		}
	}

	public void ProjectileFire()
	{
		if (OnProjectileFire != null)
		{
			OnProjectileFire();
		}
	}

	public void WindState()
	{
		if (OnWindState != null)
		{
			OnWindState();
		}
	}
}
