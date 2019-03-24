using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

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
    public event AbilityEventHandler OnNoneState;

	public event AbilityEventHandler OnEarthCrash;
    #endregion

    
    [HideInInspector] public bool IsAimToShoot;
    [HideInInspector] public float InitialGravityScale;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public float OriginalMass;
	[Range(0.1f, 3.0f)] public float PlayerElementalDmgMultiplier;				// Multiplier for player's damage on enemies

    private GameObject playerGO;
    private Player player;

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
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerRb = playerGO.GetComponent<Rigidbody>();
		OriginalMass = playerRb.mass;
        player = playerGO.GetComponent<Player>();
        IsAimToShoot = true;
#elif UNITY_PS4
        IsAimToShoot = false;
        PS4Input.PadSetLightBar(0, 255, 0, 0);
#endif
    }


#if UNITY_PS4
    public IEnumerator ControllerVibration(bool isLargeVibration)
    {
        if (isLargeVibration)
            PS4Input.PadSetVibration(0, 70, 0);
        else
            PS4Input.PadSetVibration(0, 0, 80);

        yield return new WaitForSeconds(0.2f);
        PS4Input.PadSetVibration(0, 0, 0);
    }
#endif

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
#if UNITY_PS4
            PS4Input.PadSetLightBar(0, 255, 255, 255);
#endif
            player.SetElement(ElementalStates.Wind);
			OnWindState();
		}
	}

	// Method for running methods subscribed to OnFireState event
	// All fire state methods are run via this function
	public void FireState()
	{
        if (OnFireState != null)
		{
#if UNITY_PS4
            PS4Input.PadSetLightBar(0, 255, 220, 0);
#endif
            player.SetElement(ElementalStates.Fire);
            OnFireState();
		}
	}

	// Method for running methods subscribed to OnWaterState event
	// All water state methods are run via this function
	public void WaterState()
	{
        if (OnWaterState != null)
		{
#if UNITY_PS4			
            PS4Input.PadSetLightBar(0, 50, 50, 255);
#endif			
            player.SetElement(ElementalStates.Water);
            OnWaterState();
		}
	}

	// Method for running methods subscribed to OnEarthState event
	// All earth state methods are run via this function
	public void EarthState()
	{
        if (OnEarthState != null)
		{
#if UNITY_PS4			
            PS4Input.PadSetLightBar(0, 0, 255, 0);
#endif			
            player.SetElement(ElementalStates.Earth);
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

    // Method for running methods subscribed to OnNoneState event
    // This function is called when the players state is set to none
    public void NoneState()
    {
        if (OnNoneState != null)
        {
            OnNoneState();
        }
    }
}
