/* using System.Collections;
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
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickup : MonoBehaviour
{

    [SerializeField] private GameObject _onEarthCollectPE;
    [SerializeField] private float cooldownTime = 0;
    [SerializeField] private float _followSpeed = 0.05f;
    [SerializeField] private float _deleteAtRange;

    private Vector3 _velocity = Vector3.zero;
    private AbilityManager _abilityManager;
    private Player _player;
    private Animator _pickupAnim;
    private float cooloffTime;
    private bool followingPlayer = false;

	void Start () {
		_abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _pickupAnim = GetComponent<Animator>();
    }
	
	// If wind pickup interacts with player,
	// set players elemental state to Wind
	// and run method for running OnWindState event


    private void Update() {
        if (followingPlayer) {
            float dist = Vector3.Distance(transform.position, _player.transform.position);

            if (dist > _deleteAtRange) 
            {
                
                transform.position = Vector3.SmoothDamp(transform.position, _player.transform.position, ref _velocity, 0.3f);
            } 
            else 
            {
                StartCoroutine(WaitToCollect());
                followingPlayer = false;
            }
        }
    }

	private void OnTriggerStay(Collider col)
	{
        Debug.Log("In trigger");
		if (col.CompareTag("Player") && Input.GetButtonDown("Interact") && cooloffTime < Time.time)
		{
			Debug.Log("Interacted");
			PickupElement();
		}
	}

	private void PickupElement() 
	{
        _pickupAnim.SetTrigger("Collect");
        StartCoroutine(FollowAfterAnimation());
        _player.AddArmourSlot();
        _abilityManager.FireState();
	}

    private IEnumerator WaitToCollect( )
    {
        yield return new WaitForSeconds(0.3f);
        GameObject onEarthCollect = Instantiate(_onEarthCollectPE, transform.position, Quaternion.identity);
        cooloffTime = Time.time + cooldownTime;
        Destroy(onEarthCollect, 1.5f);
        Destroy(gameObject);
    }

    private IEnumerator FollowAfterAnimation() {  
        yield return new WaitForSeconds(0.8f);
        _pickupAnim.enabled = false;
        Debug.Log("Begin following player");
        followingPlayer = true;
    }
}

