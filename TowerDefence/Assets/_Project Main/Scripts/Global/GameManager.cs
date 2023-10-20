using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public TMP_Text timerText;
    private float currentTime;

    // 박준오
    public Dictionary<string, List<string>> monsterData = new Dictionary<string, List<string>>();
    // 이경민 CSV 파일 Read
    public Dictionary<string, List<string>> bossData = new Dictionary<string, List<string>>();

    #region 싱글턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        // CSV 파일을 읽어오는 기능
    }       // Awake()
    #endregion

    /*
     * 게임 매니저 작성 방법
     * 예)
     * { 박준오
     * 내용
     * } 박준오
     */


    private void Start()
    {
        monsterData = CSVReader.instance.ReadCSVFile("MonsterData");
        bossData = CSVReader.instance.ReadCSVFile("BossData");
        StartCoroutine(Late());
    }

    private IEnumerator Late()
    {
        yield return new WaitForSeconds(5);

        MonsterSpawn.instance.SetWave();
        Debug.Log("첫번째 스폰");
        yield return new WaitForSeconds(20);

        MonsterSpawn.instance.SetWave();
        Debug.Log("두번째 스폰");
    }
    // } 박준오
}
