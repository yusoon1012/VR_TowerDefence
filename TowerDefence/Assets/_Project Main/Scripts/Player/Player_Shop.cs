using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Shop : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public bool isShopIn = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer=GetComponent<LineRenderer>();
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
    }
}
