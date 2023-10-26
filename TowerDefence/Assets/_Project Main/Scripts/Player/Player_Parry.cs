using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Parry : MonoBehaviour
{
    public AudioSource sweepAudio;
    public TrailRenderer trailRenderer;
    public ParticleSystem particle;
    FinalBoss boss;
    public Transform playerTransform;
    public Transform bossTransform;
    float speed;
    public bool isParriable = false;
    private AudioSource parrySound;
    // Start is called before the first frame update
    void Start()
    {
        boss = FindAnyObjectByType<FinalBoss>();
        parrySound=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
        if (speed >= 1f)
        {
            Debug.Log("1보다 빠르다");
            
            isParriable = true;
            if (sweepAudio.isPlaying == false)
            {

                sweepAudio.Play();
            }
        }
        else
        {
            isParriable = false;
        }
        if (speed >= 0.3f)
        {
                trailRenderer.enabled = true;
            
        }
        else
        {
            trailRenderer.enabled = false;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            if (isParriable)
            {
                // 콜라이더에 들어온 오브젝트의 ThrowSpell 스크립트를 참조한다
                ThrowSpell throwSpell = other.GetComponent<ThrowSpell>();
                Rigidbody rb = other.GetComponent<Rigidbody>();
                // ThrowSpell 의 Reverse 함수를 실행해 발사체가 날아가는 방향을 바꿔준다
                throwSpell.Reverse();
                if (rb != null)
                {
                    Vector3 dir = (bossTransform.position - playerTransform.position).normalized;

                    rb.velocity = Vector3.zero;
                    rb.AddForce(dir * 100f, ForceMode.Impulse);
                    particle.Play();
                    parrySound.Play();
                }
            }
        }     // OnTriggerStay()
    }
}
