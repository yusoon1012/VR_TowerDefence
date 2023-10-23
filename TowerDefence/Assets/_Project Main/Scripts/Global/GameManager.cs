using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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

    #region 이경민
    // CSV 파일 Read
    public Dictionary<string, List<string>> bossData = new Dictionary<string, List<string>>();
    // 게임 오버 투명 빨간색 스크린
    public Image gameOverScreen;
    // 게임 클리어 투명 흰색 스크린
    public Image gameClearScreen;
    // 게임 오버 텍스트
    public Text gameOverText;
    // 게임 클리어 텍스트
    public Text gameClearText;
    // 게임 재시작 버튼
    public Button restartButton;
    // 게임 종료 버튼
    public Button exitButton;
    // 게임 오버 상태 체크
    public bool isGameOver = false;

    // 게임 클리어 실행
    public void GameClear()
    {
        isGameOver = true;
        gameClearScreen.gameObject.SetActive(true);

        GameOverFunction();

        StartCoroutine(GameClearDelay());
    }     // GameClear()

    // 게임 오버 실행
    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.gameObject.SetActive(true);

        GameOverFunction();

        StartCoroutine(GameOverDelay());
    }     // GameOver()

    // 게임 재시작 실행
    public void Restart()
    {
        isGameOver = false;
        SceneManager.LoadScene("Boss_Test");
    }     // Restart()

    // 게임 종료 실행
    public void GameExit()
    { 
        Application.Quit();
    }     // GameExit()

    // 게임 종료시 실행될 기능들
    public void GameOverFunction()
    {

    }     // GameOverFunction()

    // 게임 오버 후 잠시 동안 딜레이 타임
    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(1.5f);

        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }     // GameOverDelay()

    // 게임 클리어 후 잠시 동안 딜레이 타임
    private IEnumerator GameClearDelay()
    {
        yield return new WaitForSeconds(1.5f);

        gameClearText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }     // GameClearDelay()
    #endregion 이경민
}
