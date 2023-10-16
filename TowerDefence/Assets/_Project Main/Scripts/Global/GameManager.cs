using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region �̱���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion

    /*
     * ���� �Ŵ��� �ۼ� ���
     * ��)
     * { ���ؿ�
     * ����
     * } ���ؿ�
     */

    // { ���ؿ�
    public Dictionary<string, List<string>> monsterData = new Dictionary<string, List<string>>();

    private void Start()
    {
        StartCoroutine(Late());

        monsterData = CSVReader.instance.ReadCSVFile("MonsterData");
    }

    private IEnumerator Late()
    {
        yield return new WaitForSeconds(5);

        MonsterSpawn.instance.SetWave();
    }
    // } ���ؿ�
}
