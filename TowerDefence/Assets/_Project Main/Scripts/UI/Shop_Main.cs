using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;


public class Shop_Main : MonoBehaviour
{
   
    public AudioClip buttonClip;
    public Player_Shop playerShop;
    public GameObject shopObj;
    public GameObject[] shopPages;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Button leftButton;
    public Button rightButton;
    public GameObject buffPage;
    public GameObject unitPage;
    private int pageIndex = 0;
    private AudioSource audioSource;
   
    // Start is called before the first frame update
    void Start()
    {
        shopObj.SetActive(false);

        leftArrow.SetActive(false);
      
        for(int i=0;i<shopPages.Length; i++) 
        {
            if(i!=0)
            {
                shopPages[i].SetActive(false);
            }
        }
        audioSource=GetComponent<AudioSource>();
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

    public void PrevPage()
    {
        if(pageIndex>0)
        {
           
            shopPages[pageIndex].SetActive(false);
            pageIndex -= 1;
            if (pageIndex == 0)
            {
                leftArrow.SetActive(false);
            }
            shopPages[pageIndex].SetActive(true);

            Debug.LogFormat("PAGE의 인덱스 {0}", pageIndex);
        }
        
        rightArrow.SetActive(true);

        audioSource.clip = buttonClip;
        audioSource.Play();

    }

    public void NextPage()
    {
        if (pageIndex < shopPages.Length-1)
        { 
            shopPages[pageIndex].SetActive(false);
            pageIndex += 1;
            if(pageIndex== shopPages.Length-1)
            {
                rightArrow.SetActive(false);
            }
            shopPages[pageIndex].SetActive(true);

            Debug.LogFormat("PAGE의 인덱스 {0}", pageIndex);

        }
       
        leftArrow.SetActive(true);
        audioSource.clip = buttonClip;
        audioSource.Play();

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
