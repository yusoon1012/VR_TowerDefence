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
    public AudioClip[] shootClip;
    public AudioClip impactClip;
    public AudioClip[] countingClip;
    public AudioSource shootAudio;
    private AudioSource countingAudio; 
    Player_Shop shop;
    // Start is called before the first frame update
    void Start()
    {
        shop=GetComponent<Player_Shop>();
        countingAudio=GetComponent<AudioSource>();
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
        float horizontalInput = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch);
        transform.Rotate(new Vector3(0, horizontalInput*30, 0));
        if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger,ARAVRInput.Controller.RTouch))
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
           
            

                attackEnforceCount += 1;
            if( attackEnforceCount==5)
            {
                GameObject strongMagic = Instantiate(strongMagicPrefab, staffTop.position, staffRotation);
                attackEnforceCount = 0;
                shootAudio.clip = impactClip;
                shootAudio.Play();

                ARAVRInput.PlayVibration(shootRate / 2, 1000, 1000, ARAVRInput.Controller.RTouch);

            }
            else
            {
                int randomIdx = Random.Range(0, 3);
                shootAudio.clip = shootClip[randomIdx];
                countingAudio.clip= countingClip[attackEnforceCount-1];
                shootAudio.Play();
                countingAudio.Play();
                 GameObject magic = Instantiate(magicPrefab, staffTop.position, staffRotation);
                ARAVRInput.PlayVibration(shootRate / 2, 50, 50, ARAVRInput.Controller.RTouch);

            }
        }
    }
}
