using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraScript : MonoBehaviour {

    public Transform target; 
    [SerializeField] private GameObject[] possibleTargets = new GameObject[7];
    public float speed = 1.5f;
    
    private int currentTarget = 0;
    private int previousTarget;
    private int nextTarget;
    //private int previousTarget;
    Quaternion newRot;
    Vector3 relPos;


    private void Start()
    {
        //possibleTargets = GameObject.FindGameObjectsWithTag("Door");
        possibleTargets[0] = GameObject.Find("DungeonCentre");
        possibleTargets[1] = GameObject.Find("WindCentre");
        possibleTargets[2] = GameObject.Find("WaterCentre");
        possibleTargets[3] = GameObject.Find("FireCentre");
        possibleTargets[4] = GameObject.Find("EarthCentre");
        possibleTargets[5] = GameObject.Find("Boss Door");
        possibleTargets[6] = GameObject.Find("Shop");

        target = possibleTargets[currentTarget].transform;
    }

    // Update is called once per frame
    void Update ()
    {       
            
        GetNewTarget();
        
        relPos = target.position - transform.position;
        newRot = Quaternion.LookRotation(relPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, speed);
        //Debug.Log(possibleTargets[nextTarget]);
            
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
