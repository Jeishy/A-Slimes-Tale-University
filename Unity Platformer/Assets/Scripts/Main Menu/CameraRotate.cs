using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    [SerializeField] private float _rotateSpeed;

    private Transform trans;
    // Use this for initialization
    void Start () {
        trans = transform;
    }
	
	// Update is called once per frame
	void Update () {
        trans.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }
}
