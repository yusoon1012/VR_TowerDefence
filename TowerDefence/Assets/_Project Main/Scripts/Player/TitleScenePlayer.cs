using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScenePlayer : MonoBehaviour
{
    LineRenderer lineRenderer;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask =1<< LayerMask.NameToLayer("UI");
        lineRenderer= GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, ARAVRInput.RHandPosition);

        Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100f, layerMask))
        {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(1, hit.point);
            Button button=hit.collider.gameObject.GetComponent<Button>();
            if(button != null)
            {
                if(ARAVRInput.GetDown(ARAVRInput.Button.One,ARAVRInput.Controller.RTouch)|| ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
                {
                    button.onClick.Invoke();
                }
            }
            
        }
        else
        {
            lineRenderer.enabled = false;

        }
    }
}
