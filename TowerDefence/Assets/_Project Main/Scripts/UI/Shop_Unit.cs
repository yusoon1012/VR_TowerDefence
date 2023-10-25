using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Unit : MonoBehaviour
{
    public static Shop_Unit instance;

    Shop_Main mainShop; // 상점창을 닫기 위함

    private float bombCooltime = 0f;
    private float bladeCooltime = 0f;
    private float shootBossCoolTime = 0f;
    // 네 번째 아이콘은 무엇?
    private float unitCoolTimeRate = 15f; // 유닛 기본 쿨타임
    public GameObject[] unitIcon; // 쿨타임 슬라이더 보유 오브젝트
    public Slider[] unitSlider; // 쿨타임 슬라이더
    public Button[] unitBuy; // 구매 버튼
    private const int BOMB = 0;
    private const int BLADE = 1;
    private const int SHOOT_BOSS = 2;
    public bool buildBomb = false;
    public bool buildBlade = false;
    public bool buildShootBoss = false;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}

     buildBomb = false;
     buildBlade = false;
     buildShootBoss = false;
    }

    private void Start()
    {
        mainShop = transform.GetComponent<Shop_Main>();
    }

    #region 폭탄 유닛 구매 시 
    public void UnitBomb()
    {
        mainShop.ExitShop(); // 상점창을 닫은 후 바로 배치 
        buildBomb = true; // 설치 단계로 넘어감

        // 폭발 유닛 버튼호출 함수
        if (bombCooltime == 0)
        {
            // TODO: 구매 버튼 클릭 시 해당 메서드로 연결 되도록
            bombCooltime = unitCoolTimeRate;
            StartCoroutine(UnitBombRoutine());
        }
    }

    private IEnumerator UnitBombRoutine()
    {
        unitIcon[BOMB].SetActive(true); // 슬라이더 오브젝트 활성화
        unitBuy[BOMB].interactable = false; // 구매 버튼 비활성화

        while (bombCooltime > 0)
        {
            bombCooltime -= 1;
            yield return new WaitForSeconds(1);
            unitSlider[BOMB].value = bombCooltime / unitCoolTimeRate;
        }

        unitIcon[BOMB].SetActive(false);
        unitBuy[BOMB].interactable = true; // 재구매 가능
    }
    #endregion

    #region 근거리 타격 유닛
    public void UnitBlade()
    {
        mainShop.ExitShop(); // 상점창을 닫은 후 바로 배치 
        buildBlade = true; // 설치 단계로 넘어감

        // 근거리 타격 유닛 유닛 버튼호출 함수
        if (bladeCooltime == 0)
        {
            // TODO: 구매 버튼 클릭 시 해당 메서드로 연결 되도록
            bladeCooltime = unitCoolTimeRate;
            StartCoroutine(UnitBladeRoutine());
        }
    }

    private IEnumerator UnitBladeRoutine()
    {
        unitIcon[BLADE].SetActive(true); // 슬라이더 오브젝트 활성화
        unitBuy[BLADE].interactable = false; // 구매 버튼 비활성화

        while (bladeCooltime > 0)
        {
            bladeCooltime -= 1;
            yield return new WaitForSeconds(1);
            unitSlider[BLADE].value = bladeCooltime / unitCoolTimeRate;
        }

        unitIcon[BLADE].SetActive(false);
        unitBuy[BLADE].interactable = true; // 재구매 가능
    }
    #endregion

    #region 보스 타격 유닛
    public void UnitShootBoss()
    {
        mainShop.ExitShop(); // 상점창을 닫은 후 바로 배치 
        buildShootBoss = true; // 설치 단계로 넘어감

        // 폭발 유닛 버튼호출 함수
        if (shootBossCoolTime == 0)
        {
            Debug.Log("쿨타임 코루틴 작동 시도");

            // TODO: 구매 버튼 클릭 시 해당 메서드로 연결 되도록
            shootBossCoolTime = unitCoolTimeRate;
            StartCoroutine(UnitShootBossRoutine());
        }
    }

    private IEnumerator UnitShootBossRoutine()
    {
        Debug.Log("쿨타임 코루틴 작동 시도");

        unitIcon[SHOOT_BOSS].SetActive(true); // 슬라이더 오브젝트 활성화
        unitBuy[SHOOT_BOSS].interactable = false; // 구매 버튼 비활성화

        while (shootBossCoolTime > 0)
        {
            shootBossCoolTime -= 1;
            yield return new WaitForSeconds(1);
            unitSlider[SHOOT_BOSS].value = shootBossCoolTime / unitCoolTimeRate;
        }

        unitIcon[SHOOT_BOSS].SetActive(false);
        unitBuy[SHOOT_BOSS].interactable = true; // 재구매 가능
    }
    #endregion
}
