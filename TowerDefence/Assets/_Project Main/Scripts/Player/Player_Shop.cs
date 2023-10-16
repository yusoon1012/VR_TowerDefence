using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Shop : MonoBehaviour
{
    private Shop_Main shopMain;
    private LineRenderer lineRenderer;
    public bool isShopOpen = false;
    public bool isShopIn = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer=GetComponent<LineRenderer>();
        shopMain=FindAnyObjectByType<Shop_Main>();  
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
        RaycastHit hit = default;
        int uiLayer = 1 << LayerMask.NameToLayer("UI");
        if (Physics.Raycast(ray, out hit,50f, uiLayer))
        {
            isShopIn = true;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0,ARAVRInput.RHandPosition);
            lineRenderer.SetPosition(1, hit.point);
            Button button=hit.collider.gameObject.GetComponent<Button>();
            if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.RTouch))
            {

            if (button != null )
            {
                button.onClick.Invoke();
            }
            }
        }
        else
        {
            lineRenderer.enabled = false;
            isShopIn = false;

        }

        if(ARAVRInput.GetDown(ARAVRInput.Button.One,ARAVRInput.Controller.LTouch))
        {
            if(isShopOpen==false)
            {
                isShopOpen = true;
                shopMain.ActiveShop();
            }
            else
            {
                isShopOpen = false;
                shopMain.ExitShop();
            }
        }
    }
}
