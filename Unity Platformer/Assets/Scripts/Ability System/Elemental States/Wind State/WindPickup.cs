using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPickup : MonoBehaviour {

    [SerializeField] private GameObject _onWindCollectPE;

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
    private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
            _pickupAnim.SetTrigger("Wind");
            StartCoroutine(WaitToCollect());
            _player.AddArmourSlot();
            _abilityManager.WindState();
			Destroy(gameObject, 1f);
		}
	}

    private IEnumerator WaitToCollect()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject onWindCollect = Instantiate(_onWindCollectPE, transform.position, Quaternion.identity);
        Destroy(onWindCollect, 1f);
    }
}
