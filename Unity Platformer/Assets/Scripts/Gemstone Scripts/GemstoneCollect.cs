using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemstoneCollect : MonoBehaviour {


    public GameObject OnCollectPE;
    [HideInInspector] public bool IsCollected;

    [SerializeField] private float _smoothTime;
    [SerializeField] private float _rotSpeed;

    private Transform _trans;
    private Transform _gemstoneFollowTrans;
    private Transform _playerTrans;
    private Vector3 _velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        _trans = transform;
        _gemstoneFollowTrans = GameObject.Find("GemstoneFollow").transform;
        _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
	    if (IsCollected)
        {
            // Smoothly transition position, from previous to gemstonefollow gameobject's position
            transform.position = Vector3.SmoothDamp(transform.position, _gemstoneFollowTrans.position, ref _velocity, _smoothTime);
            // Calculate new rotation
            var newRotation = Quaternion.LookRotation(transform.position - _playerTrans.position, -Vector3.forward);
            // Set x and y components to 0
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            // Apply slerp between old and new calculated rotations over time
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * _rotSpeed);
        }
	}
}
