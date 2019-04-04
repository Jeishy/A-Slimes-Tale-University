using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPickup : MonoBehaviour
{
    [SerializeField] private Animator _textAnim;
    [SerializeField] private ElementalStates element;
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
    private AudioManager _audioManager;
    private Vector3 preAnimationPos;

	void Start () {
        _audioManager = AudioManager.instance;
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
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _textAnim.SetTrigger("Open");
        }
    }

	private void OnTriggerStay(Collider col)
	{
		if (col.CompareTag("Player"))
		{       
            //Display text 'Press E To Absorb'  
            if (Input.GetButtonDown("Interact")) {
                

                if (cooloffTime < Time.time) {
                    Debug.Log("Interacted");
                    PickupElement();
                }
            }
		}
	}

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _textAnim.SetTrigger("Close");
        }
    }

	private void PickupElement() 
	{
        preAnimationPos = transform.position;
        _pickupAnim.SetTrigger("Collect");
        StartCoroutine(FollowAfterAnimation());
        _player.AddArmourSlot();
        _abilityManager.SetState(element);
	}

    private IEnumerator WaitToCollect( )
    {
        yield return new WaitForSeconds(0.3f);
        _audioManager.Play("ElementPickup");
        _audioManager.Stop("ElementFlying");
        GameObject onEarthCollect = Instantiate(_onEarthCollectPE, transform.position, Quaternion.identity);
        cooloffTime = Time.time + cooldownTime;
        Destroy(onEarthCollect, 1.5f);
        _pickupAnim.enabled = true;
        _pickupAnim.SetTrigger("Regenerate");
        yield return new WaitForSeconds(0.5f);
        transform.position = preAnimationPos;
    }

    private IEnumerator FollowAfterAnimation() {  
        _audioManager.Play("ElementFlying");
        yield return new WaitForSeconds(0.8f);
        _pickupAnim.enabled = false;
        Debug.Log("Begin following player");
        followingPlayer = true;
    }
}
