using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점에서 버프형 유닛 구매 여부를 가져와 적용하는 클래스
/// </summary>
public class UnitBuySystem : MonoBehaviour
{
    // 상점(유닛창) 게임오브젝트
    [SerializeField] GameObject shop = default;
    Shop_Buff shop_Buff;
    // Shop_Buff 클래스에서 가져온 활성화 버프 인덱스
    private int attack_Enforce = 0;
    private int unit_Duration = 1;
    private int damage_Up = 2;
    // 활성화 여부를 알리는 bool 
    public static bool active_attackEnforce = false;
    public static bool active_unitDuration = false;
    public static bool active_damageUp = false;         

    private void Awake()
    {
        shop_Buff = shop.transform.GetComponent<Shop_Buff>();
    }

    private void Update()
    {
        #region 버프형 유닛 구매 시 
        if (shop_Buff.buffIcon[unit_Duration].activeSelf) // 유닛 지속시간 증가 
        {
            active_unitDuration = true;
        }
        else active_unitDuration = false;

        if (shop_Buff.buffIcon[damage_Up].activeSelf) // 유닛 공격력 증가 
        {
            active_damageUp = true;
        }
        else active_damageUp = false;
        #endregion

        #region 설치형 유닛 구매 시 
        if (shop_Buff.buffIcon[attack_Enforce].activeSelf) // 폭탄 유닛
        {
            active_attackEnforce = true;
        }
        else active_attackEnforce = false;
        #endregion
    }
}
