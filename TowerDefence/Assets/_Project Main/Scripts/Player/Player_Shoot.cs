using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    public Transform crossHair;
    public Transform staffTop;
    public Transform staffRoot;
    public GameObject magicPrefab;
    public GameObject strongMagicPrefab;
    private float shootingCoolTime = 0;
    private float shootRate = 1.1f;
    private int attackEnforceCount = 0;
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
        shootingCoolTime += Time.deltaTime;
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.RTouch))
        {
            if(shootingCoolTime<shootRate)
            {
                return;
            }
            else
            {
                shootingCoolTime = 0;
            }
            Vector3 handRotation = ARAVRInput.RHandDirection;
            Quaternion staffRotation=Quaternion.LookRotation(handRotation);
            if(Shop_Buff.instance.isAttackEnforce)
            {
                attackEnforceCount += 1;
            }

            if(Shop_Buff.instance.isAttackEnforce&&attackEnforceCount==3)
            {
                GameObject strongMagic = Instantiate(strongMagicPrefab, staffTop.position, staffRotation);
                attackEnforceCount = 0;
            }
            else
            {
            GameObject magic = Instantiate(magicPrefab, staffTop.position, staffRotation);

            }
        }
    }
}
