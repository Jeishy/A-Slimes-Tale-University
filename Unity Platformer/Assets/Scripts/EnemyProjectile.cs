using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

	private ElementalStates elementalState = ElementalStates.None;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}

	public void SetElement(ElementalStates element) {
		elementalState = element;
	}

	public ElementalStates GetElement() {
		return elementalState;
	}
}
