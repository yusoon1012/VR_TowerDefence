#define PC
#define Oculus
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// UnitBuySystem => 설치/공격형 & 버프형 유닛 적용과 설치 
/// </summary>
public class UnitBuildSystem : MonoBehaviour
{
    // 전체 설치/공격형 유닛 리스트
    public static List<GameObject> units = new List<GameObject>();
    // 풀 포지션
    private Vector3 poolPos = new Vector3(0f, -10f, 0f);
    // 유닛 설치 위치 
    private Vector3 buildPos = default;

    #region 게임 오브젝트 변수
    // 타워 프리팹
    [SerializeField] private GameObject bombUnitPrefab, bladeUnitPrefab, bossShootUnitPrefab = default;
    // 타워 오브젝트
    private GameObject[] bombUnit, bladeUnit, shootBossUnit = default;
    // 선택 시의 유닛 프리팹
    [SerializeField] private GameObject selectBombPrefab, selectBladePrefab, selectShootBossPrefab = default;
    // 선택 시의 유닛 오브젝트 
    private GameObject selectBombUnit, selectBladeUnit, selectShootBossUnit = default;
    // 선택 허용 시 빛기둥 프리팹
    [SerializeField] GameObject blueLightPrefab = default;
    // 선택 중복 시 빛기둥 프리팹
    [SerializeField] GameObject redLightPrefab = default;
    // 빛기둥
    private GameObject blueLight = default;
    private GameObject redLight = default;
    private GameObject lightEffect = default;
    // 플레이어
    [SerializeField]private GameObject player;
    #endregion

    #region 설치를 위한 변수
    Shop_Unit shop_Unit = new Shop_Unit();
    // 폭탄 유닛 설치 여부 
    private bool buildBombUnit = false;
    public float unitLifeTime = 10f; // 유닛 지속시간
    // 유닛 설치 위치 (중복 설치X)
    private List<Vector3> unitBuildPos = new List<Vector3>();
    // 설치 가능 Material
    private Material green = default;
    // 설치 불가 Material
    private Material red = default;
    // 설치될 유닛
    private GameObject unit = default;
    // 설치될 유닛의 목표 위치
    private GameObject selectUnit = default;
    // 그리드 셀의 크기
    private float gridSize = 1.0f; // 그리드 셀의 크기
    // 설치된 유닛 개수 (종류별)
    int bombCount = 0;
    int shootBossCount = 0;
    int bladeCount = 0;
    #endregion

    private void Awake()
    {
        // 플레이어
        player = GameObject.Find("Player");

        // 배열 지정 
        bombUnit = new GameObject[3];
        shootBossUnit = new GameObject[2];
        bladeUnit = new GameObject[1];

        // 유닛 생성 
        for (int i = 0; i < 3; i++) // 폭탄 유닛 3개
        {
            bombUnit[i] = Instantiate(bombUnitPrefab, poolPos, bombUnitPrefab.transform.rotation);
        }
        for (int i = 0; i < 2; i++) // 보스 타격 유닛 2개
        {
            shootBossUnit[i] = Instantiate(bossShootUnitPrefab, poolPos, bossShootUnitPrefab.transform.rotation);
        }
        for (int i = 0; i < 1; i++) // 근거리 타격 유닛 1개
        {
            bladeUnit[i] = Instantiate(bladeUnitPrefab, poolPos, bladeUnitPrefab.transform.rotation);
        }

        // 선택 시 유닛 오브젝트 생성
        selectBombUnit = Instantiate(selectBombPrefab, poolPos, selectBombPrefab.transform.rotation);
        selectBladeUnit = Instantiate(selectBladePrefab, poolPos, selectBladePrefab.transform.rotation);
        selectShootBossUnit = Instantiate(selectShootBossPrefab, poolPos, selectShootBossPrefab.transform.rotation);

        // Material
        green = Resources.Load<Material>("Material/Green");
        red = Resources.Load<Material>("Material/Red");

        // 선택 시 빛기둥 오브젝트 생성
        blueLight = Instantiate(blueLightPrefab, poolPos, blueLightPrefab.transform.rotation);
        redLight = Instantiate(redLightPrefab, poolPos, redLightPrefab.transform.rotation);

        shop_Unit = GameObject.Find("Shop_").GetComponent<Shop_Unit>(); // 유닛 구매 여부를 알기 위함
    }

    private void Update()
    {
        #region 유닛 공격력 상승
        if (Shop_Buff.instance.buffIcon[0])
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().damage *= 1.5f; // 공격력 * 1.5f (50% 상승)
            }
        }
        else if (!Shop_Buff.instance.buffIcon[0])
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().damage /= 1.5f; // 공격력 / 2.0f (원상태 복귀)
            }
        }
        #endregion

        
#region 유닛 공격속도 상승
        if (Shop_Buff.instance.buffIcon[1])
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().speed *= 1.5f; // 공격속도 * 1.5f (50% 상승)
            }
        }
        else if (!Shop_Buff.instance.buffIcon[1])
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().speed /= 1.5f; // 공격속도 / 1.5f (원상태 복귀)
            }
        }
        #endregion
        #region 유닛 지속시간 증가
        //if (Shop_Buff.instance.isUnitDuration)
        //{
        //    unitLifeTime += 2; // 지속시간 두 배 증가 
        //}
        //else unitLifeTime = 10f; // 초기화
        #endregion

        #region 유닛 설치
        if (shop_Unit.buildBomb) // 폭발 유닛 설치 (3개까지)
        {
            if (bombCount >= 3)
            {
                bombCount = 0;
            }

            unit = bombUnit[bombCount];
            selectUnit = selectBombUnit;
            Unit_Range();
        }

        if (shop_Unit.buildShootBoss) // 보스 타격 유닛 (2개만)
        {
            if (shootBossCount >= 2)
            {
                shootBossCount = 0;
            }

            unit = shootBossUnit[shootBossCount];
            //shootBossUnit[shootBossCount].GetComponent<UnitAttack_ShootBoss>().enabled = true;
            selectUnit = selectShootBossUnit;
            Unit_Range();
        }

        if (shop_Unit.buildBlade) // 근거리 타격 유닛 설치 (1개까지)
        {
            unit = bladeUnit[0];
            selectUnit = selectBladeUnit;
            Unit_Range();
        }
        #endregion
    }

    /// <summary>
    /// 유닛 설치를 위한 좌표를 구함
    /// </summary>
    private Vector3 SelectPosition
    {
        get
        {
            Vector3 selectPos = default; // 컨트롤러 커서 위치
            Vector3 targetPos = default; // 실 설치 위치

            int floorLayer = 1 << LayerMask.NameToLayer("Ground");
            // Ray를 카메라의 위치로부터 나가도록 한다.
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo = default;

            if (Physics.Raycast(ray, out hitInfo, 200f, floorLayer))
            {
                Debug.Log("땅을 감지!");
                selectPos = hitInfo.point;
            }

            targetPos = new Vector3(Mathf.Floor(selectPos.x / gridSize) * gridSize, 0.5f,
            Mathf.Floor(selectPos.z / gridSize) * gridSize);

            return targetPos; // 이후 셀포지션을 return 하도록 수정
        }
    }

    #region 유닛 설치 
    /// <summary>
    /// 유닛 배치 범위를 표시
    /// </summary>
    /// <param name="unitNum">구매한 유닛 번호</param>
    public void Unit_Range()
    {
        bool overlap = false; // 유닛 위치 중복 여부 체크
        buildPos = SelectPosition; // 유닛을 설치할 좌표

        // 유닛 중복 위치 금지
        foreach (Vector3 location in unitBuildPos)
        {
            if (location == selectUnit.transform.position) // 중복 확인 시 
            {
                Transform obj = selectUnit.transform.GetChild(0);  // 실오브젝트
                MeshRenderer[] children = obj.GetComponentsInChildren<MeshRenderer>(); // 구성 요소 Material 배열
                lightEffect = redLight;

                for (int i = 0; i < children.Length; i++)
                {
                    children[i].material = red;
                }

                overlap = true;
            }
            else // 중복이 아니라면 
            {
                Transform obj= selectUnit.transform.GetChild(0);  // 실오브젝트
                MeshRenderer[] children = obj.GetComponentsInChildren<MeshRenderer>(); // 구성 요소 Material 배열
                lightEffect = blueLight;

                for (int i = 0; i < children.Length; i++)
                {
                    children[i].material = green;
                }

                overlap = false;
            }
        }

        Vector3 playerPos = player.transform.position;
        playerPos.y = 0.5f; // 직선거리 계산을 위한 y축 고정
        Vector3 selectpos = SelectPosition;
        selectpos.y = 0.5f;

        if (Vector3.Distance(playerPos, selectpos) < 30) // 설치형 유닛 PC 기준 직선거리 30m 설치 제한 기준, 중복X
        {
            if (overlap)
            {
                redLight.transform.position = buildPos;
                blueLight.transform.position = poolPos;
            }
            else if (!overlap)
            {
                blueLight.transform.position = buildPos;
                redLight.transform.position = poolPos;
            }

            selectUnit.transform.position = buildPos;

            // 설치 키를 입력했으며 중복 설치 시도가 아니라면
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch) && !overlap)
            {
                blueLight.transform.position = poolPos;
                redLight.transform.position = poolPos;

                selectUnit.transform.position = poolPos; // 위치 표시 유닛은 풀로 복귀

                Unit_Build(); // 타워 설치
            }
        }
    }


    /// <summary>
    /// 유닛을 배치
    /// </summary>
    private void Unit_Build()
    {
        // 타워 설치 
        unit.transform.position = buildPos;
        unitBuildPos.Add(unit.transform.position); // 유닛 설치 위치 등록

        CalculateUnitCount();

        // 임시 유지시간 20초. 폭발 유닛 빼고 
        //Invoke("Unit_TimeOver", unitLifeTime); // 유닛 유지시간 동안 대기 후 오브젝트 풀로 유닛 이동  
    }

    /// <summary>
    /// 설치 유닛 수 계산
    /// </summary>
    private void CalculateUnitCount()
    {
        if (shop_Unit.buildBomb) // 3개까지
        {
            bombCount++;
            shop_Unit.buildBomb = false;
        }
        else if (shop_Unit.buildShootBoss) // 2개까지
        {
            shootBossCount++;
            shop_Unit.buildShootBoss = false;
        }
        else if (shop_Unit.buildBlade) // 1개까지
        {
            // 0번 인덱스만 사용하므로 추가 동작X
            shop_Unit.buildBlade = false;
        }
    }

    private void Unit_TimeOver() 
    {
        unitBuildPos.Remove(unit.transform.position); // 유닛 설치 위치 제거
        unit.transform.position = poolPos;
    }
    #endregion

    /// <summary>
    /// 유닛이 풀로 복귀
    /// </summary>
    public void ReturnPool(GameObject unit)
    {
        unit.transform.position = poolPos;
    }

    /// <summary>
    /// HP 0인 유닛을 회수
    /// </summary>
    private void UnitReturn()
    {

    }
}
