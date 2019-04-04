using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowGemstoneCount : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _gemstoneTxt;

    private void Update()
    {
        _gemstoneTxt.text = "Gemstones collected: " + GameManager.instance.gemstones;
    }
}
