using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraScript : MonoBehaviour {

    public Transform target;
    [SerializeField] private GameObject[] possibleTargets;
    public float speed = 3f;
    
    private int currentTarget = 3;
    private int previousTarget;
    private int nextTarget;
    //private int previousTarget;
    Quaternion newRot;
    Vector3 relPos;


    private void Start()
    {
        possibleTargets = GameObject.FindGameObjectsWithTag("Door");
        target = possibleTargets[currentTarget].transform;
    }

    // Update is called once per frame
    void Update ()
    {       
            
        GetNewTarget();
        
        relPos = target.position - transform.position;
        newRot = Quaternion.LookRotation(relPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, speed);
            
    }

    void GetNewTarget()
    {
        
        
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget < possibleTargets.Length)
            {
                if(currentTarget == possibleTargets.Length - 1)
                { 
                    return;
                }
                currentTarget++;
                nextTarget = currentTarget;
                
            }
            target = possibleTargets[nextTarget].transform;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentTarget > 0)
            {

                currentTarget--;
                nextTarget = currentTarget;

            }
            target = possibleTargets[nextTarget].transform;
        }

        


    }

}
