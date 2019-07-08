using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBook : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        Open();
    }

    public void Credits()
    {
        _anim.SetTrigger("Credits");
    }

    public void StartGame()
    {
        LevelChanger.instance.OnLevelComplete();
    }

    public void Open()
    {
        _anim.SetTrigger("Open");
    }

    public void Quitgame()
    {
        LevelChanger.instance.FadeToLevel(1);
        Application.Quit();
    }
}
