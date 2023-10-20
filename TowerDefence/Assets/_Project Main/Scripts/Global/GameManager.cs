using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public TMP_Text timerText;
    public float currentTime;
    public int minute;
    public float second;
    private float goldTimer = 0f;
    private float goldRate = 10f;

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
    private void Update()
    {
       //second += Time.deltaTime;
        currentTime += Time.deltaTime;
        minute = (int)currentTime / 60;
        second = currentTime% 60;
       
        
        if(second<9.5f)
        {

        timerText.text=string.Format("{0:N0} : 0{1:N0}",minute,second);
        }
        else if(second>59.5)
        {
            timerText.text = string.Format("{0:N0} : 00", minute, second);

        }
        else
        {
            timerText.text = string.Format("{0:N0} : {1:N0}", minute, second);

        }
        goldTimer += Time.deltaTime;
        if(goldTimer>=goldRate)
        {
            GoldManager.instance.TimeAddGold();
            goldTimer = 0;
        }
        


    }
   

    private IEnumerator Late()
    {
        yield return new WaitForSeconds(5);

        MonsterSpawn.instance.SetWave();
    }
    // } 박준오
}
