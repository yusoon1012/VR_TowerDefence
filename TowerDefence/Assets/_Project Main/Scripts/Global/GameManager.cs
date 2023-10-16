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
    // } 박준오
}
