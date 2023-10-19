using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region 싱글턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        // CSV 파일을 읽어오는 기능
        monsterData = CSVReader.instance.ReadCSVFile("MonsterData");
        bossData = CSVReader.instance.ReadCSVFile("BossData");
    }       // Awake()
    #endregion

    /*
     * 게임 매니저 작성 방법
     * 예)
     * { 박준오
     * 내용
     * } 박준오
     */

    // { 박준오
    public Dictionary<string, List<string>> monsterData = new Dictionary<string, List<string>>();
    // 이경민 CSV 파일 Read
    public Dictionary<string, List<string>> bossData = new Dictionary<string, List<string>>();

    private void Start()
    {
        //StartCoroutine(Late());
    }

    private IEnumerator Late()
    {
        yield return new WaitForSeconds(5);

        MonsterSpawn.instance.SetWave();
    }
    // } 박준오
}
