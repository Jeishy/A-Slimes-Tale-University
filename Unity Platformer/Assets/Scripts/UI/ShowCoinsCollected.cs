using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCoinsCollected : MonoBehaviour {

    private TextMeshProUGUI _coinsCollectedTxt;

    private void Start()
    {
        _coinsCollectedTxt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update () {
        _coinsCollectedTxt.text = "Coins Collected: " + GameManager.instance.collectibles;

    }
}
