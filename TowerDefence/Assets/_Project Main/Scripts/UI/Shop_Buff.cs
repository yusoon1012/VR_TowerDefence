using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Shop_Buff : MonoBehaviour
{
    public AudioClip buyClip;
    public static Shop_Buff instance;
    public Sprite[] healImages;
    public GameObject[] buffIcon;
    public Slider[] buffSlider;
    public bool isAttackEnforce = false;
    public bool isDamageUp = false;
    public bool isUnitDuration = false;
    private bool isHealing = false;
    private const int POWER_UP = 0;
    private const int ATTACK_SPEED = 1;
    private const int HEAL = 2;
    private int buffCount = 0;
    private float damageUpCooltime = 0f;
    private float attackSpeedUpCooltime = 0f;
    
   
    private float damageUpTimeRate;
    private float attackSpeedTimeRate;

    private int healPrice;
    private int attackSpeedPrice;
    private int damageUpPrice;
    private AudioSource audioSource;

    private bool[] isDamageUpBuy=new bool[3];
    private bool[] isHealingBuy = new bool[3];
    private bool[] isAttackSpeedBuy = new bool[3];
    const int LV_1 = 0;
    const int LV_2 = 1;
    const int LV_3 = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        isAttackEnforce = false;
        isDamageUp = false;
        isUnitDuration = false;
        isHealing = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buffIcon.Length; i++)
        {
            buffIcon[i].SetActive(false);
        }
        audioSource= GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnitDamageUp(int level)
    {
        switch (level)
        {
            case 1:
                damageUpPrice = 250;
                damageUpTimeRate = 10f;
                if (isDamageUpBuy[LV_1]==true)
                {
                    return;
                }
                else
                {
                    isDamageUpBuy[LV_1] = true;
                }

                break;
            case 2:
                damageUpPrice = 500;
                damageUpTimeRate = 15f;
                if (isDamageUpBuy[LV_2] == true)
                {
                    return;
                }
                else
                {
                    isDamageUpBuy[LV_2] = true;
                }
                break;
            case 3:
                damageUpPrice = 750;
                damageUpTimeRate = 20f;
                if (isDamageUpBuy[LV_3] == true)
                {
                    return;
                }
                else
                {
                    isDamageUpBuy[LV_3] = true;
                }
                break;
            default:
                break;
        }
        if(GoldManager.instance.currentGold<damageUpPrice)
        {
            return;
        }
        //유닛 공격력 상승시키는 버튼호출 함수
        if (damageUpCooltime == 0)
        {

            audioSource.clip=buyClip;
            audioSource.Play();
            GoldManager.instance.BuyThings(damageUpPrice);
            damageUpCooltime = damageUpTimeRate;
            buffIcon[POWER_UP].SetActive(true);
            StartCoroutine(UnitDamageRoutine(level));
        }

    }

    public void UnitAttackSpeedUp(int level)
    {
        switch (level)
        {
            case 1:
                attackSpeedPrice = 250;
                attackSpeedTimeRate = 10f;
                if (isAttackSpeedBuy[LV_1] == true)
                {
                    return;
                }
                else
                {
                    isAttackSpeedBuy[LV_1] = true;
                }
                break;
            case 2:
                attackSpeedPrice = 500;
                attackSpeedTimeRate = 15f;
                if (isAttackSpeedBuy[LV_2] == true)
                {
                    return;
                }
                else
                {
                    isAttackSpeedBuy[LV_2] = true;
                }
                break;
            case 3:
                attackSpeedPrice = 750;
                attackSpeedTimeRate = 20f;
                if (isAttackSpeedBuy[LV_3] == true)
                {
                    return;
                }
                else
                {
                    isAttackSpeedBuy[LV_3] = true;
                }

                break;
            default:
                break;
        }
        if (GoldManager.instance.currentGold < attackSpeedPrice)
        {
            return;
        }
        //유닛 공격속도 상승시키는 버튼호출 함수
        if (attackSpeedUpCooltime == 0)
        {
            audioSource.clip = buyClip;
            audioSource.Play();
            GoldManager.instance.BuyThings(attackSpeedPrice);
            attackSpeedUpCooltime = attackSpeedTimeRate;
            buffIcon[ATTACK_SPEED].SetActive(true);
            StartCoroutine(UnitAttackSpeedUpRoutine(level));

        }

    }

    public void PlayerHeal(int level)
    {
        switch (level)
        {
            case 1:
                healPrice = 300; 
                if (isHealingBuy[LV_1] == true)
                {
                    return;
                }
                else
                {
                    isHealingBuy[LV_1] = true;
                }
                break;
            case 2:
                healPrice = 400;
                if (isHealingBuy[LV_2] == true)
                {
                    return;
                }
                else
                {
                    isHealingBuy[LV_2] = true;
                }
                break;
            case 3:
                healPrice = 500;
                if (isHealingBuy[LV_3] == true)
                {
                    return;
                }
                else
                {
                    isHealingBuy[LV_3] = true;
                }
                break;
            default:
                break;
        }

        if(GoldManager.instance.currentGold<healPrice)
        {
            return;
        }
        if (isHealing == false)
        {
            audioSource.clip = buyClip;
            audioSource.Play();
            GoldManager.instance.BuyThings(healPrice);
            AudioSource audio=gameObject.GetComponent<AudioSource>();
            if(audio != null)
            {
            audio.Play();
            }
            isHealing = true;
            Image img = buffSlider[HEAL].GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = healImages[level - 1];
            }
            
            buffIcon[HEAL].SetActive(true);
            StartCoroutine(HealRoutine(level));
        }
    }

    private IEnumerator UnitAttackSpeedUpRoutine(int level_)
    {
        while (attackSpeedUpCooltime > 0)
        {
            attackSpeedUpCooltime -= 1;
            yield return new WaitForSeconds(1);
            buffSlider[ATTACK_SPEED].value = attackSpeedUpCooltime / attackSpeedTimeRate;
        }
        switch (level_)
        {
            case 1:
                isAttackSpeedBuy[LV_1] = false;
                break;
            case 2:
                isAttackSpeedBuy[LV_2] = false;

                break;
            case 3:
                isAttackSpeedBuy[LV_3] = false;

                break;
            default:
                break;
        }
        buffIcon[ATTACK_SPEED].SetActive(false);
    }
    private IEnumerator UnitDamageRoutine(int level_)
    {
        //TODO : 유닛 데미지 업 함수 호출
        while (damageUpCooltime > 0)
        {
            damageUpCooltime -= 1;
            yield return new WaitForSeconds(1);
            buffSlider[POWER_UP].value = damageUpCooltime / damageUpTimeRate;
        }
        switch(level_)
        {
            case 1:
                isDamageUpBuy[LV_1] = false;
                break;
                case 2:
                isDamageUpBuy[LV_2] = false;

                break;
                case 3:
                isDamageUpBuy[LV_3] = false;

                break;
            default:
                break;
        }
        buffIcon[POWER_UP].SetActive(false);

    }
    private IEnumerator HealRoutine(int level)
    {
        float healTime = 5;
        float lastTime;
        while (healTime > 0)
        {
            yield return new WaitForSeconds(1);
            lastTime = healTime;
            healTime -= 1;
            buffSlider[HEAL].value = healTime / 5;
            Player_Status.Instance.PlayerHeal(level);
        }
        //buffSlider[HEAL].value = healTime / 5;
        buffIcon[HEAL].SetActive(false);
        isHealing = false;
    }

    //! LEGACY
    //public void DamageBuff()
    //{
    //    if (attackDamageCoolTime == 0)
    //    {

    //        Player_Status.Instance.playerDamage += 1;
    //        buffIcon[DAMAGE_UP].SetActive(true);
    //        attackDamageCoolTime = buffCoolTimeRate;
    //        StartCoroutine(DamageBuffRoutine());

    //    }
    //}
    //public void UnitDuration()
    //{
    //    if (unitDurationCoolTime == 0)
    //    {

    //        buffIcon[UNIT_DURATION].SetActive(true);
    //        unitDurationCoolTime = buffCoolTimeRate;

    //        StartCoroutine(UnitDurationRoutine());
    //    }

    //}
    //public void AttackEnforce()
    //{
    //    if (attackEnforceCoolTime == 0)
    //    {
    //        isAttackEnforce = true;
    //        buffIcon[ATTACK_ENFORCE].SetActive(true);
    //        attackEnforceCoolTime = buffCoolTimeRate;
    //        StartCoroutine(AttackEnforceRoutine());
    //    }
    //}

    //private IEnumerator DamageBuffRoutine()
    //{
    //    while(attackDamageCoolTime>0)
    //    {
    //        attackDamageCoolTime -= 1;
    //        buffSlider[DAMAGE_UP].value = attackDamageCoolTime / buffCoolTimeRate;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    buffIcon[DAMAGE_UP].SetActive(false);

    //}

    //private IEnumerator UnitDurationRoutine()
    //{
    //    while(unitDurationCoolTime>0)
    //    {
    //        unitDurationCoolTime -= 1;
    //        buffSlider[UNIT_DURATION].value=unitDurationCoolTime / buffCoolTimeRate;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    buffIcon[UNIT_DURATION].SetActive(false);
    //}

    //private IEnumerator AttackEnforceRoutine()
    //{
    //    while (attackEnforceCoolTime > 0)
    //    {
    //        attackEnforceCoolTime -= 1;
    //        buffSlider[ATTACK_ENFORCE].value = attackEnforceCoolTime / buffCoolTimeRate;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    buffIcon[ATTACK_ENFORCE].SetActive(false);
    //    isAttackEnforce = false;

    //}
}
