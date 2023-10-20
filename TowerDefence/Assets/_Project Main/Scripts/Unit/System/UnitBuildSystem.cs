#define PC
#define Oculus
using System.Collections;
using System.Collections.Generic;
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
    // 그리드
    [SerializeField] private Grid grid;
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
            bombUnit[i] = Instantiate(bombUnitPrefab, poolPos, Quaternion.identity);
        }
        for (int i = 0; i < 2; i++) // 보스 타격 유닛 2개
        {
            shootBossUnit[i] = Instantiate(bossShootUnitPrefab, poolPos, Quaternion.identity);
        }
        for (int i = 0; i < 1; i++) // 근거리 타격 유닛 1개
        {
            bladeUnit[i] = Instantiate(bladeUnitPrefab, poolPos, Quaternion.identity);
        }
        // 선택 시 유닛 오브젝트 생성
        selectBombUnit = Instantiate(selectBombPrefab, poolPos, Quaternion.identity);
        selectBladeUnit = Instantiate(selectBladePrefab, poolPos, Quaternion.identity);
        selectShootBossUnit = Instantiate(selectShootBossPrefab, poolPos, Quaternion.identity);

        // Material
        green = Resources.Load<Material>("Material/Green");
        red = Resources.Load<Material>("Material/Red");

        shop_Unit = GameObject.Find("Shop").GetComponent<Shop_Unit>(); // 유닛 구매 여부를 알기 위함

        //shootBossUnit.GetComponent<UnitAttack_ShootBoss>().enabled = true; // Issue: 비활성화 문제로 넣은 코드 
    }

    private void Update()
    {
        // TODO: unit 배분 
        // 폭발 유닛은 3개까지
        // 보스 타격 유닛은 2개까지
        // 근거리 타격 유닛은 1개만 
        // 상점에서 구매 버튼을 누른다면 위 int 값을 수정하는 것으로 개수 조절. 

        #region 유닛 공격력 증가
        //if (Shop_Buff.instance.isDamageUp)
        //{
        //    foreach(GameObject unit in units)
        //    {
        //        unit.GetComponent<AttackUnitProperty>().damage *= 2; // 공격력 * 2
        //    }
        //}
        //else
        //{
        //    foreach (GameObject unit in units)
        //    {
        //        unit.GetComponent<AttackUnitProperty>().damage /= 2; // 공격력 / 2 (원상태 복귀)
        //    }
        //}
        
        #endregion

        #region 유닛 지속시간 증가
        //if (Shop_Buff.instance.isUnitDuration)
        //{
        //    unitLifeTime += 2; // 지속시간 두 배 증가 
        //}
        //else unitLifeTime = 10f; // 초기화
        #endregion

        if (shop_Unit.buildBomb) // 폭발 유닛 설치
        {
            unit = bombUnit[0];
            selectUnit = selectBombUnit;
            Unit_Range();
        }

        if (shop_Unit.buildBlade) // 근거리 타격 유닛 설치
        {
            unit = bladeUnit[0];
            selectUnit = selectBladeUnit;
            Unit_Range();
        }

        if (shop_Unit.buildShootBoss) // 보스 타격 유닛
        {
            unit = shootBossUnit[0];
            selectUnit = selectShootBossUnit;
            Unit_Range() ;
        }
    }

    /// <summary>
    /// 유닛 설치를 위한 좌표를 구함
    /// </summary>
    private Vector3 SelectPosition
    {
        get
        {
            Vector3 selectPos = default;
            int floorLayer = 1 << LayerMask.NameToLayer("Ground");
            // Ray를 카메라의 위치로부터 나가도록 한다.
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection); // 왜 안되나...?
            RaycastHit hitInfo = default;

            if (Physics.Raycast(ray, out hitInfo, 200f, floorLayer))
            {
                Debug.Log("땅과의 충돌을 감지!");
                selectPos = hitInfo.point;
            }

            Vector3Int gridPos = grid.WorldToCell(selectPos);

            return gridPos; // 이후 셀포지션을 return 하도록 수정
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
        buildPos.y += unit.transform.GetComponent<Collider>().bounds.size.y / 2;

        // 유닛 중복 위치 금지
        foreach (Vector3 location in unitBuildPos)
        {
            if (location == selectUnit.transform.position) // 중복 확인 시 
            {
                Transform obj = selectUnit.transform.GetChild(0);  // 실오브젝트
                MeshRenderer[] children = obj.GetComponentsInChildren<MeshRenderer>(); // 구성 요소 Material 배열
                
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
            selectUnit.transform.position = buildPos;

            // 설치 키를 입력했으며 중복 설치 시도가 아니라면
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch) && !overlap)
            {
                shop_Unit.buildBomb = false;
                shop_Unit.buildBlade = false;
                shop_Unit.buildShootBoss = false;

                Unit_Build(); // 타워 설치
            }
        }
    }

    /// <summary>
    /// 유닛을 배치
    /// </summary>
    private void Unit_Build()
    {
        selectUnit.transform.position = poolPos; // 위치 표시 유닛은 풀로 복귀 
        // 타워 설치 
        unit.transform.position = buildPos;
        unitBuildPos.Add(unit.transform.position); // 유닛 설치 위치 등록

        // 임시 유지시간 20초. 폭발 유닛 빼고 
        //Invoke("Unit_TimeOver", unitLifeTime); // 유닛 유지시간 동안 대기 후 오브젝트 풀로 유닛 이동  
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
}
