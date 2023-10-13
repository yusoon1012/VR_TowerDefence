using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    public Transform crossHair;
    public Transform staffTop;
    public Transform staffRoot;
    public GameObject magicPrefab;
    Player_Shop shop;
    // Start is called before the first frame update
    void Start()
    {
        shop=GetComponent<Player_Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shop.isShopIn)
        {
            return;
        }
        ARAVRInput.DrawCrosshair(crossHair);
       
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.RTouch))
        {
            Vector3 handRotation = ARAVRInput.RHandDirection;
            Quaternion staffRotation=Quaternion.LookRotation(handRotation);
            GameObject magic = Instantiate(magicPrefab, staffTop.position, staffRotation);
        }
    }
}
