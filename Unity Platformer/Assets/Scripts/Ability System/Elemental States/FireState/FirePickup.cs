using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickup : MonoBehaviour {

    [SerializeField] private GameObject _onFireCollectPE;
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
            _pickupAnim.SetTrigger("Fire");
            StartCoroutine(WaitToCollect());
            _player.AddArmourSlot();
            _abilityManager.FireState();
			Destroy(gameObject, 1f);
		}
	}

    private IEnumerator WaitToCollect()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject onFireCollect = Instantiate(_onFireCollectPE, transform.position, Quaternion.identity);
        Destroy(onFireCollect, 1f);
    }
}
