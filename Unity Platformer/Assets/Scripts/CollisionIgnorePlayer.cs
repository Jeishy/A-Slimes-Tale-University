using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnorePlayer : MonoBehaviour {
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

	private void OnCollisionEnter(Collision collision)
    {
        Collider hitCol = collision.gameObject.GetComponent<Collider>();
        if (hitCol.CompareTag("Player"))
        {
            Debug.Log("Player collider ignored!");
            Physics.IgnoreCollision(col, hitCol);
        }
    }
}
