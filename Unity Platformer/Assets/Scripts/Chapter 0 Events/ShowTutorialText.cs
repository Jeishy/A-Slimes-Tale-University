using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorialText : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private StartCutscene _cutscene;

    // Start is called before the first frame update
    private void OnEnable()
    {
        _cutscene.OnCutsceneEnd += ShowText;
    }

    private void OnDisable()
    {
        _cutscene.OnCutsceneEnd += ShowText;
    }

    private void ShowText()
    {
        _anim.SetTrigger("Show");
    }
}
