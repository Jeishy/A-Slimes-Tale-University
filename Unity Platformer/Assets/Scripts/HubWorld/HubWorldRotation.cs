using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldRotation : MonoBehaviour {
    public GameObject []TriggerPoints;
    private Transform []TriggerPositions;
    private GameObject target;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

    void Start()
    {
        /*for (int i = 0; i < TriggerPoints.Length; i++)
        {
            TriggerPoints[i].GetComponent<Transform>();
            Debug.Log(TriggerPoints[i]);
        }*/

        TriggerPositions = new Transform[TriggerPoints.Length];
        //Debug.Log(TriggerPositions);

        for (int i = 0; i < TriggerPoints.Length; i++)
        {
            TriggerPositions[i] = TriggerPoints[i].transform;
            
        }
        Debug.Log(TriggerPositions);

        //target = GameObject.FindWithTag("Player");
    }

    /*void Update()
    {
        targetPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }*/


}
