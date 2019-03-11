using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOnTrigger : MonoBehaviour {

    public Text text;
    private bool textFading;

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == GameObject.FindGameObjectWithTag("Player"))
        {
            textFading = true;
            text.CrossFadeAlpha(1, 2f, false);
        }
        /*else
        {
            textFading = false;
            text.CrossFadeAlpha(0, 2f, false);
        }*/
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision == GameObject.FindGameObjectWithTag("Player"))
        {
            textFading = false;
            text.CrossFadeAlpha(0, 2f, false);
        }
    }
}
