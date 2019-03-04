using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHubWorld : MonoBehaviour
{
    
    
    //Script adapated from multiple shops to a single shop. Usually would change what items are shown dependent on what shop the player goes to but in this case
    //It just displays items for a single shop which can be edited from the Global Shop Script.
    
    public GameObject ShopRef;
    public GameObject ShopInventory;
    public GameObject Item01Text;
    public GameObject Item02Text;
    public GameObject Item03Text;
    public GameObject Item04Text;
    public GameObject Item05Text;
    public GameObject Item06Text;
    public GameObject ItemCompletion;
    public GameObject CompleteText;

    
  



    private void Awake()
    {

        ShopRef.GetComponent<GlobalShop>().ShopHub = true; //Activates the shop
        
    }

    private void Update()
    {
        Transform theShop = GameObject.Find("Shop").transform;
        Transform lookingAtShop = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TargetCameraScript>().target;
        if (lookingAtShop == theShop)
        {
            Debug.Log("Activate the Shop");
            ShopActivator();
        }
        else { ShopInventory.SetActive(false); }
    }

    /*private void OnTriggerEnter2D(Collider2D col)
    {        
        Debug.Log("Collision Detected");
        ShopInventory.SetActive(true); //Activates the shop panel UI
        Debug.Log("Shop Panel is now active");
        Item01Text.GetComponent<Text>().text = "" + GlobalShop.Item01; 
        Item02Text.GetComponent<Text>().text = "" + GlobalShop.Item02;
        Item03Text.GetComponent<Text>().text = "" + GlobalShop.Item03;
        Item04Text.GetComponent<Text>().text = "" + GlobalShop.Item04;
        Item05Text.GetComponent<Text>().text = "" + GlobalShop.Item05;
        Item06Text.GetComponent<Text>().text = "" + GlobalShop.Item06;

        // Previously designed so that you could have multiple shops in different parts of the level and it would have different items
        //  stripped back and modified for just a single hub world shop 
    }*/


    private void ShopActivator() //Activated if the Camera is currently targetting the shop
    {
        //Debug.Log("Collision Detected");
        ShopInventory.SetActive(true); //Activates the shop panel UI
        Debug.Log("Shop Panel is now active");
        Item01Text.GetComponent<Text>().text = "" + GlobalShop.Item01;
        Item02Text.GetComponent<Text>().text = "" + GlobalShop.Item02;
        Item03Text.GetComponent<Text>().text = "" + GlobalShop.Item03;
        Item04Text.GetComponent<Text>().text = "" + GlobalShop.Item04;
        Item05Text.GetComponent<Text>().text = "" + GlobalShop.Item05;
        Item06Text.GetComponent<Text>().text = "" + GlobalShop.Item06;
    }
    
   /*private void OnTriggerExit2D(Collider2D other)
{
        ShopInventory.SetActive(false);
}*/
    
    //Not currently very effecient way of writing if they are sure they want to buy the item they have picked.

   
    public void Item01()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item01 + "?";
    }

    public void Item02()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item02 + "?";
    }

    public void Item03()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item03 + "?";
    }

    public void Item04()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item04 + "?";
    }

    public void Item05()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item05 + "?";
    }

    public void Item06()
    {
        ItemCompletion.SetActive(true);
        CompleteText.GetComponent<Text>().text = "Are you sure you want to buy " + GlobalShop.Item06 + "?";
    }

    public void CancelTransaction()
    {
        ItemCompletion.SetActive(false);
    }

}
