using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Player_Status : MonoBehaviour
{
    // 싱글톤 패턴을 적용할 클래스의 인스턴스를 저장하는 변수
    private static Player_Status _instance;
    public float playerCurrentHp;
    public float playerMaxHp = 50;
    public int playerDamage = 1;
    public Slider hpBar;
    public GameObject postProcess;

    // 다른 스크립트에서 이 클래스의 인스턴스에 액세스할 수 있는 프로퍼티
    public static Player_Status Instance
    {
        get { return _instance; }
    }

    void Start()
    {
        // 만약 인스턴스가 이미 설정되어 있다면, 현재 인스턴스를 파괴한다.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // 인스턴스가 설정되지 않았다면, 이 스크립트의 인스턴스를 설정한다.
            _instance = this;
        }
        playerCurrentHp = playerMaxHp;
        hpBar.value = playerCurrentHp / playerMaxHp;
        DotDamaged(10);
    }
   
    public void PlayerHeal(int level)
    {
        switch (level)
        {
            case 1: 
                if(playerCurrentHp+20>playerMaxHp)
                {
                    playerCurrentHp=playerMaxHp;
                }
                else
                {
                    playerCurrentHp += 20;
                }
                break;
            case 2:
                if (playerCurrentHp + 30 > playerMaxHp)
                {
                    playerCurrentHp = playerMaxHp;
                }
                else
                {
                    playerCurrentHp += 30;
                }
                break;
            case 3:
                if (playerCurrentHp + 40 > playerMaxHp)
                {
                    playerCurrentHp = playerMaxHp;
                }
                else
                {
                    playerCurrentHp += 40;
                }
                break;
            default:
                break;
        }
        hpBar.value = playerCurrentHp / playerMaxHp;

    }

    public void PlayerDamaged(int damage)
    {
        if (GameManager.instance.isGameOver == true) { return; }

        playerCurrentHp -= damage;
        hpBar.value = playerCurrentHp / playerMaxHp;

        // 플레이어의 남은 HP 가 0 보다 작거나 같으면
        if (playerCurrentHp <= 0)
        {
            // 게임 매니저의 GameOver 함수를 실행시킨다
            GameManager.instance.GameOver();
        }
    }     // PlayerDamaged()

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Meteor"))
        {
            ThrowSpell throwSpell=collision.gameObject.GetComponent<ThrowSpell>();
            if(throwSpell != null)
            {
                if(throwSpell.missileType==ThrowSpell.MissileType.BLIND) 
                {

                    postProcess.SetActive(true);
                    StartCoroutine(BlindRoutine());
                }
            }
        }
    }

    private IEnumerator BlindRoutine()
    {
        yield return new WaitForSeconds(5);
        postProcess.SetActive(true);
    }


    public void DotDamaged(int damage)
    {
        StartCoroutine(DotRoutine(damage));
        
    }
    private IEnumerator DotRoutine(int damage_)
    {
        float lastHp= playerCurrentHp;
        while (lastHp - damage_ < playerCurrentHp)
        {
            yield return new WaitForSeconds(1);
            playerCurrentHp -= 1;
            hpBar.value = playerCurrentHp / playerMaxHp;

        }
        hpBar.value = playerCurrentHp / playerMaxHp;

    }
}
