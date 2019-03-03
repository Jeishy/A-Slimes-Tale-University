using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraScript : MonoBehaviour {

    public Transform target;
    public float speed = 3f;
    
    private int currentTarget = 0;
    private int previousTarget;
    private int nextTarget;
    //private int previousTarget;
    Quaternion newRot;
    Vector3 relPos;
	
	
	// Update is called once per frame
	void Update ()
    {       
            GetNewTarget();
        if (target == true)
        {
            relPos = target.position - transform.position;
            newRot = Quaternion.LookRotation(relPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, speed);
        }    
    }

    void GetNewTarget()
    {
        GameObject[] possibleTargets;
        possibleTargets = GameObject.FindGameObjectsWithTag("Door");
        Debug.Log(possibleTargets[currentTarget]);

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
            
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentTarget > 0)
            {

                currentTarget--;
                nextTarget = currentTarget;

            }
        }
        
            target = possibleTargets[nextTarget].transform;

        
    }

}
