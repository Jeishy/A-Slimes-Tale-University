using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] private CharacterController2D m_CharacterController2D;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            m_CharacterController2D.m_Grounded = true;
        }
    }
}
