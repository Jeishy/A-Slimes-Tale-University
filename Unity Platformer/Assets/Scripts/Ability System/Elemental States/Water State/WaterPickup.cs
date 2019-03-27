using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPickup : MonoBehaviour {

    [SerializeField] private GameObject _onWaterCollectPE;
#if UNITY_PS4
    [SerializeField] private AudioManager _audioManager;
#endif

    private AbilityManager _abilityManager;
    private Player _player;
    private Animator _pickupAnim;

    void Start () {
		_abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _pickupAnim = GetComponent<Animator>();
    }

    // If wind pickup interacts with player,
    // set players elemental state to Wind
    // and run method for running OnWindState event
    private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
#if UNITY_PS4
            _audioManager.PlayPS4("ElementalPickupCollect");
#endif
            _pickupAnim.SetTrigger("Water");
            StartCoroutine(WaitToCollect());
            _player.AddArmourSlot();
            _abilityManager.WaterState();
			Destroy(gameObject, 1f);
		}
	}

    private IEnumerator WaitToCollect()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject onWaterCollect = Instantiate(_onWaterCollectPE, transform.position, Quaternion.identity);
        Destroy(onWaterCollect, 1f);
    }
}
