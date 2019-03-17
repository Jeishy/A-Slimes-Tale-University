using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmourSlotGUI : MonoBehaviour
{

    [SerializeField] private Sprite[] _armourCounters;
    [SerializeField] private Image _armourCounterSlot1;
    [SerializeField] private Image _armourCounterSlot2;
    [SerializeField] private GameObject _armourBar1;
    [SerializeField] private GameObject _armourBar2;
    [Space] [SerializeField] private Color _fireColour;
    [SerializeField] private Color _waterColour;
    [SerializeField] private Color _windColour;
    [SerializeField] private Color _earthColour;

    private Player _player;
    private AbilityManager _abilityManager;

	// Use this for initialization
	private void OnEnable ()
    {
        Setup();

        _abilityManager.OnFireState += SetFireColour;
        _abilityManager.OnWaterState += SetWaterColour;
        _abilityManager.OnWindState += SetWindColour;
        _abilityManager.OnEarthState += SetEarthColour;
    }

    private void OnDisable()
    {
        _abilityManager.OnFireState -= SetFireColour;
        _abilityManager.OnWaterState -= SetWaterColour;
        _abilityManager.OnWindState -= SetWindColour;
        _abilityManager.OnEarthState -= SetEarthColour;
    }

    private void Setup()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (_player.health > 0)
        {
            if (_player.armour > 0)
                SetArmourCounters();
            else if (_player.armour <= 0)
                SetArmourToZero();
        }
	}

    private void SetArmourCounters()
    {
        // Set armour slot one
        if (_player.armour > 3)
        {
            _armourBar2.SetActive(true);
            _armourBar1.SetActive(true);
            _armourCounterSlot1.sprite = _armourCounters[3];
            _armourCounterSlot2.sprite = _armourCounters[_player.armour - 3];
        }
        // Set armour slot two
        else if (_player.armour <= 3)
        {
            _armourBar2.SetActive(false);
            _armourBar1.SetActive(true);
            _armourCounterSlot2.sprite = _armourCounters[0];
            _armourCounterSlot1.sprite = _armourCounters[_player.armour];
        }
    }

    private void SetArmourToZero()
    {
        _armourCounterSlot2.sprite = _armourCounters[0];
        _armourCounterSlot1.sprite = _armourCounters[0];
        _armourBar2.SetActive(false);
        _armourBar1.SetActive(false);
    }

    private void SetFireColour()
    {
        SetColour(_fireColour);
    }

    private void SetWaterColour()
    {
        SetColour(_waterColour);
    }

    private void SetWindColour()
    {
        SetColour(_windColour);
    }

    private void SetEarthColour()
    {
        SetColour(_earthColour);

    }

    public void SetColour(Color colour)
    {
        _armourCounterSlot2.color = colour;
        _armourCounterSlot1.color = colour;
        _armourBar2.GetComponent<Image>().color = colour;
        _armourBar1.GetComponent<Image>().color = colour;
    }
}
