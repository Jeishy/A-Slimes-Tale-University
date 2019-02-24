using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShop : MonoBehaviour {

    public static string Item01;
    public static string Item02;
    public static string Item03;
    public static string Item04;
    public static string Item05;
    public static string Item06;
    public bool ShopHub = false;

    // Update is called once per frame
    void Update()
    {

        if (ShopHub == true)
        {
            
            Item01 = "PlaceHolder1";
            Item02 = "PlaceHolder2";
            Item03 = "PlaceHolder3";
            Item04 = "PlaceHolder4";
            Item05 = "PlaceHolder5";
            Item06 = "PlaceHolder6";
        }
        

    }
}
