using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GoldManager : MonoBehaviour
{
    public TMP_Text goldText;
    public int currentGold=0;
    public int startGold = 250;
    private int timeGold = 15;
    private int monsterGold = 20;
    public static GoldManager instance = null;
    AudioSource audio;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        currentGold = startGold;
        goldText.text = string.Format("{0}", currentGold);
        audio=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimeAddGold()
    {
        int targetGold = currentGold + 15;       
        StartCoroutine(GoldRoutine(targetGold));
    }
    public void EnemyDropGold()
    {
        int targetGold = currentGold + 20;
        StartCoroutine(GoldRoutine(targetGold));

    }
    public void BossDamageGold(int damage_)
    {
        int targetGold = currentGold + (damage_/2) ;
        StartCoroutine(GoldRoutine(targetGold));
    }
    public void BuyThings(int gold)
    {
        currentGold -= gold;
        goldText.text = string.Format("{0}", currentGold);

    }

    private IEnumerator GoldRoutine(int targetGold_)
    {
        while(currentGold<targetGold_)
        {
            yield return new WaitForSeconds(0.01f);
            currentGold += 1;
            goldText.text = string.Format("{0}", currentGold);
        }
    }

}
