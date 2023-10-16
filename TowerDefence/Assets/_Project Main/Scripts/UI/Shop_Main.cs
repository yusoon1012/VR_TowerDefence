using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;


public class Shop_Main : MonoBehaviour
{
    public Player_Shop playerShop;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Button leftButton;
    public Button rightButton;
    public GameObject buffPage;
    public GameObject unitPage;
    // Start is called before the first frame update
    void Start()
    {
    
        
        leftArrow.SetActive(false);
        unitPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UnitPage()
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(false);
        unitPage.SetActive(true);
        buffPage.SetActive(false);
    }
    public void BuffPage()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(true);
        unitPage.SetActive(false);
        buffPage.SetActive(true);
    }
    public void ActiveShop()
    {
        gameObject.SetActive(true);
    }
    public void ExitShop()
    {
        gameObject.SetActive(false);
        playerShop.isShopOpen = false;
    }
}
