using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCavasFollow : MonoBehaviour {

    [SerializeField] private float yOffset;
    private Transform _playerTrans;
    private Transform _trans;

    // Use this for initialization
    void Start () {
        _trans = transform;
        _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        _trans.position = new Vector3(_playerTrans.position.x - 0.1f, _playerTrans.position.y + yOffset, _playerTrans.position.z);
    }
}
