using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Shop_Buff : MonoBehaviour
{
    public static Shop_Buff instance;
    
    public GameObject[] buffIcon;
    public Slider[] buffSlider;
    public bool isAttackEnforce = false;
    public bool isDamageUp = false;
    public bool isUnitDuration = false;

    private const int ATTACK_ENFORCE = 0;
    private const int UNIT_DURATION = 1;
    private const int DAMAGE_UP = 2;
    private int buffCount = 0;
    private float attackEnforceCoolTime = 0f;
    private float attackDamageCoolTime = 0f;
    private float unitDurationCoolTime = 0f;
    private float buffCoolTimeRate = 15f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buffIcon.Length; i++)
        {
            buffIcon[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageBuff()
    {
        if (attackDamageCoolTime == 0)
        {

            Player_Status.Instance.playerDamage += 1;
            buffIcon[DAMAGE_UP].SetActive(true);
            attackDamageCoolTime = buffCoolTimeRate;
            StartCoroutine(DamageBuffRoutine());

        }
    }
    public void UnitDuration()
    {
        if (unitDurationCoolTime == 0)
        {

            buffIcon[UNIT_DURATION].SetActive(true);
            unitDurationCoolTime = buffCoolTimeRate;

            StartCoroutine(UnitDurationRoutine());
        }

    }
    public void AttackEnforce()
    {
        if (attackEnforceCoolTime == 0)
        {
            isAttackEnforce = true;
            buffIcon[ATTACK_ENFORCE].SetActive(true);
            attackEnforceCoolTime = buffCoolTimeRate;
            StartCoroutine(AttackEnforceRoutine());
        }
    }

    private IEnumerator DamageBuffRoutine()
    {
        while(attackDamageCoolTime>0)
        {
            attackDamageCoolTime -= 1;
            buffSlider[DAMAGE_UP].value = attackDamageCoolTime / buffCoolTimeRate;
            yield return new WaitForSeconds(1f);
        }
        buffIcon[DAMAGE_UP].SetActive(false);

    }

    private IEnumerator UnitDurationRoutine()
    {
        while(unitDurationCoolTime>0)
        {
            unitDurationCoolTime -= 1;
            buffSlider[UNIT_DURATION].value=unitDurationCoolTime / buffCoolTimeRate;
            yield return new WaitForSeconds(1f);
        }
        buffIcon[UNIT_DURATION].SetActive(false);
    }

    private IEnumerator AttackEnforceRoutine()
    {
        while (attackEnforceCoolTime > 0)
        {
            attackEnforceCoolTime -= 1;
            buffSlider[ATTACK_ENFORCE].value = attackEnforceCoolTime / buffCoolTimeRate;
            yield return new WaitForSeconds(1f);
        }
        buffIcon[ATTACK_ENFORCE].SetActive(false);
        isAttackEnforce = false;

    }
}
