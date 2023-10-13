using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    public Transform crossHair;
    public Transform staffTop;
    public Transform staffRoot;
    public GameObject magicPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ARAVRInput.DrawCrosshair(crossHair);
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.RTouch))
        {
            GameObject magic = Instantiate(magicPrefab, staffTop.position, staffRoot.localRotation);
        }
    }
}
