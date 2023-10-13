using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    // �̱��� ������ ������ Ŭ������ �ν��Ͻ��� �����ϴ� ����
    private static Player_Status _instance;
    public int playerCurrentHp;
    public int playerMaxHp=50;
    public int playerDamage = 1;
    public TMP_Text damageStat;

    // �ٸ� ��ũ��Ʈ���� �� Ŭ������ �ν��Ͻ��� �׼����� �� �ִ� ������Ƽ
    public static Player_Status Instance
    {
        get { return _instance; }
    }


    // Start is called before the first frame update
    void Start()
    {
        // ���� �ν��Ͻ��� �̹� �����Ǿ� �ִٸ�, ���� �ν��Ͻ��� �ı��Ѵ�.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // �ν��Ͻ��� �������� �ʾҴٸ�, �� ��ũ��Ʈ�� �ν��Ͻ��� �����Ѵ�.
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        damageStat.text=string.Format("player damage {0}",Player_Status.Instance.playerDamage);
    }


}
