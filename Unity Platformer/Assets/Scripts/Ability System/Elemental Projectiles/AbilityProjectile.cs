using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProjectile : MonoBehaviour {

    public float fireRate;                                              // Used by AbilityInputHandler class to determine rate of fire

    [SerializeField] private Transform _projectileSpawnTrans;
    [SerializeField] private GameObject _fireProj;
    [SerializeField] private GameObject _waterProj;
    [SerializeField] private GameObject _windProj;
    [SerializeField] private GameObject _earthProj;
    [Space]
    [SerializeField] private GameObject _fireMuzzleFlash;
    [SerializeField] private GameObject _waterMuzzleFlash;
    [SerializeField] private GameObject _windMuzzleFlash;
    [SerializeField] private GameObject _earthMuzzleFlash;
    [Space]

    private AudioManager _audioManager;
    private AbilityManager _abilityManager;
    private Player _playerDurability;
    private Transform _playerTrans;

    private void OnEnable()
    {
        Setup();
        _abilityManager.OnProjectileFire += SpawnProjectile;
    }

    private void OnDisable()
    {
        _abilityManager.OnProjectileFire -= SpawnProjectile;
    }

    private void Setup()
    {
        _audioManager = AudioManager.instance;
        _abilityManager = GetComponent<AbilityManager>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerTrans = player.GetComponent<Transform>();
        _playerDurability = player.GetComponent<Player>();
    }

    private void SpawnProjectile()
    {
        ElementalStates state = _abilityManager.CurrentPlayerElementalState;
        // 0 = Fire, 1 = Water, 2 = Wind, 3 = Earth
        if (_playerDurability.armour > 0)
        {
            _playerDurability.armour--;

            if (_playerDurability.armour == 0 && _abilityManager.CurrentPlayerElementalState != ElementalStates.None)
            {
                Debug.Log("Calling none state");
                _abilityManager.NoneState();
            }

            switch (state)
            {
                case ElementalStates.Fire:
                    GameObject fire = Instantiate(_fireProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _audioManager.Play("FireProj");
                    fire.GetComponent<FireProjectile>().Shoot();
                    GameObject fireMf = Instantiate(_fireMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity, _playerTrans);
                    Destroy(fireMf, 1f);
                    break;
                case ElementalStates.Water:
                    GameObject water = Instantiate(_waterProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _audioManager.Play("WaterProj");
                    water.GetComponent<WaterProjectile>().Shoot();
                    GameObject waterMf = Instantiate(_waterMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity, _playerTrans);
                    Destroy(waterMf, 1f);
                    break;
                case ElementalStates.Wind:
                    GameObject wind = Instantiate(_windProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _audioManager.Play("WindProj");
                    wind.GetComponent<WindProjectile>().Shoot();
                    GameObject windMf = Instantiate(_windMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity, _playerTrans);
                    Destroy(windMf, 1f);
                    break;
                case ElementalStates.Earth:
                    GameObject earth = Instantiate(_earthProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _audioManager.Play("EarthProj");
                    earth.GetComponent<EarthProjectile>().Shoot();
                    GameObject earthMf = Instantiate(_earthMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity, _playerTrans);
                    Destroy(earthMf, 1f);
                    break;
                case ElementalStates.None:
                    // Update UI or play particle effect here
                    //Debug.Log("State is None!");
                    break;
                default:
                    Debug.LogWarning("Ability state not set!");
                    break;
            }
        }
        else
        {
            // Update UI to screen
            // Set players current state to None
            // Check if players state isn't already none
            Debug.Log("Used up all armour points! Elemental state set to none");
        }         
    }
}
