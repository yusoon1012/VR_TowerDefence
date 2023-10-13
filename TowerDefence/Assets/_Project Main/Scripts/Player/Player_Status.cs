using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    // 싱글톤 패턴을 적용할 클래스의 인스턴스를 저장하는 변수
    private static Player_Status _instance;
    public int playerCurrentHp;
    public int playerMaxHp=50;
    public int playerDamage = 1;
    public TMP_Text damageStat;

    // 다른 스크립트에서 이 클래스의 인스턴스에 액세스할 수 있는 프로퍼티
    public static Player_Status Instance
    {
        get { return _instance; }
    }


    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        damageStat.text=string.Format("player damage {0}",Player_Status.Instance.playerDamage);
    }


}
