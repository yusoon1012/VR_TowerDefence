using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;


public class Shop_Main : MonoBehaviour
{
    public Player_Shop playerShop;
    public GameObject shopObj;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Button leftButton;
    public Button rightButton;
    public GameObject buffPage;
    public GameObject unitPage;
   
    // Start is called before the first frame update
    void Start()
    {
        shopObj.SetActive(false);

        leftArrow.SetActive(false);
        buffPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UnitPage()
    {
        rightArrow.SetActive(true);
        leftArrow.SetActive(false);
        unitPage.SetActive(true);
        buffPage.SetActive(false);
    }
    public void BuffPage()
    {
        rightArrow.SetActive(false);
        leftArrow.SetActive(true);
        unitPage.SetActive(false);
        buffPage.SetActive(true);
    }
    public void ActiveShop()
    {
        shopObj.SetActive(true);

       
    }
    public void ExitShop()
    {
        shopObj.SetActive(false);
        playerShop.isShopOpen = false;
    }
}
