using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CaveSecondTurn : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Transform _hunterTrans;
    [SerializeField] private GameObject _caveInvisWall;
    [SerializeField] private CinemachineVirtualCamera _vCamCave;
    [SerializeField] private CinemachineVirtualCamera _vCamCave2;

    private HunterMovement _hunterMovement;
    // Start is called before the first frame update
    void Start()
    {
        _hunterMovement = _hunterTrans.gameObject.GetComponent<HunterMovement>();
        _caveInvisWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter"))
        {
            _charController.enabled = false;
            StartCoroutine(CaveSetup());
        }
    }

    private IEnumerator CaveSetup()
    {
        _hunterMovement.DisableMovement();
        _vCamCave2.Priority = 15;
        _hunterMovement.FindHighestPriorityCamera();
        StartCoroutine(_hunterMovement.AutomatedWalkTimed(1f));
        Quaternion lastRot = _hunterTrans.rotation;
        Quaternion newRot = Quaternion.Euler(_hunterTrans.rotation.eulerAngles + new Vector3(0.0f, -10f, 0.0f));
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime;
            _hunterTrans.rotation = Quaternion.Slerp(lastRot, newRot, time);
            yield return null;
        }
        _charController.enabled = true;
        _hunterMovement.EnableMovement();
        _vCamCave.m_Follow = null;
        _vCamCave2.m_Follow = _hunterTrans;
        yield return new WaitForSeconds(1f);
        _caveInvisWall.SetActive(true);
        gameObject.SetActive(false);
    }
}
