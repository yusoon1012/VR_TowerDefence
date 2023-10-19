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
    // 전체 유닛 리스트
    public static List<GameObject> units = new List<GameObject>();
    // 풀 포지션
    private Vector3 poolPos = new Vector3(0f, -10f, 0f);

    #region 게임 오브젝트 변수
    // 타워 프리팹
    [SerializeField] private GameObject bombUnitPrefab, bladeUnitPrefab, bossShootUnitPrefab = default;
    // 타워 오브젝트
    private GameObject bombUnit, bladeUnit, shootBossUnit = default;
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
    // 폭탄 유닛 설치 여부 
    private bool buildBombUnit = false;
    public float unitLifeTime = 10f; // 유닛 지속시간
    // 유닛 설치 위치 (중복 설치X)
    private List<Vector3> unitBuildPos = new List<Vector3>();
    // 설치 가능 Material
    private Material green = default;
    // 설치 불가 Material
    private Material red = default;
    #endregion

    private GameObject unit = default; // (test)
    private GameObject selectUnit = default; // (test)

    private void Awake()
    {
        // 플레이어
        player = GameObject.Find("Player");
        // 타워 생성 
        bombUnit = Instantiate(bombUnitPrefab, poolPos, Quaternion.identity);
        bladeUnit = Instantiate(bladeUnitPrefab, poolPos, Quaternion.identity);
        shootBossUnit = Instantiate(bossShootUnitPrefab, poolPos, Quaternion.identity);
        // 선택 시 타워 오브젝트 생성
        selectBombUnit = Instantiate(selectBombPrefab, poolPos, Quaternion.identity);
        selectBladeUnit = Instantiate(selectBladePrefab, poolPos, Quaternion.identity);
        selectShootBossUnit = Instantiate(selectShootBossPrefab, poolPos, Quaternion.identity);
        // Material
        green = Resources.Load<Material>("Material/Green");
        red = Resources.Load<Material>("Material/Red");

        unit = shootBossUnit; // (test)
        selectUnit = selectShootBossUnit; // (test)

        shootBossUnit.GetComponent<UnitAttack_ShootBoss>().enabled = true; // Issue: 비활성화 문제로 넣은 코드 
    }

    private void Update()
    {
        #region 유닛 설치
        //if (/*Shop_Buff에 폭탄유닛 bool 추가되면 넣기*/ !buildBombUnit) // 폭탄유닛를 구매 & 비활성화 중
        //{
        //    BombUnit_Range(); // 설치 범위 표시

        //    if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch)) // 입력 시 
        //    {
        //        buildBombUnit = true; // 폭탄유닛 설치 = 활성화
        //        BombUnit_Build(); // 타워 설치
        //    }
        //}
        //else if (bombUnit) // 폭탄유닛을 활성화했다면
        //{
        //    selectBombUnit.transform.position = poolPos;

        //    if (/* 폭탄 유닛 비활성 체크*/ 1 == 1) // 폭탄유닛 쿨타임 종료 시
        //    {
        //        buildBombUnit = false; // 설치 여부 초기화 
        //    }
        //}
        #endregion

        #region 테스트_유닛 설치
        Unit_Range();
        #endregion

        #region 유닛 공격력 증가
        if (Shop_Buff.instance.isDamageUp)
        {
            foreach(GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().damage *= 2; // 공격력 * 2
            }
        }
        else
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<AttackUnitProperty>().damage /= 2; // 공격력 / 2 (원상태 복귀)
            }
        }
        
        #endregion

        #region 유닛 지속시간 증가
        if (Shop_Buff.instance.isUnitDuration)
        {
            unitLifeTime += 2; // 지속시간 두 배 증가 
        }
        else unitLifeTime = 10f; // 초기화
        #endregion
    }

    /// <summary>
    /// 유닛 설치를 위한 좌표를 구함
    /// </summary>
    private Vector3 SelectPosition
    {
        get
        {
            Vector3 selectPos = default;
            int floorLayer = 1 << LayerMask.NameToLayer("Terrain");
            // Ray를 카메라의 위치로부터 나가도록 한다.
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection); // 왜 안되나...?
            RaycastHit hitInfo = default;

            if (Physics.Raycast(ray, out hitInfo, 200f, floorLayer))
            {
                selectPos = hitInfo.point;
            }

            Vector3Int gridPos = grid.WorldToCell(selectPos);

            return gridPos; // 이후 셀포지션을 return 하도록 수정
        }
    }

    #region 폭탄 유닛
    /// <summary>
    /// 유닛 배치 범위를 표시
    /// </summary>
    private void Unit_Range()
    {
        bool overlap = false; // 유닛 위치 중복 여부 체크

        Vector3 buildPos = SelectPosition; // 유닛을 설치할 좌표
        buildPos.y += 0.5f; // TODO: 선택유닛 콜라이더의 절반 높이 추가 => GetComponent<Collider>(), collider.bounds.size.y

        // 유닛 중복 위치 금지
        foreach (Vector3 location in unitBuildPos)
        {
            if (location == selectUnit.transform.position) // 중복 확인 시 
            {
                selectUnit.GetComponent<MeshRenderer>().material = red;
                overlap = true;
            }
            else // 중복이 아니라면 
            {
                selectUnit.GetComponent<MeshRenderer>().material = green;
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
        unit.transform.position = selectUnit.transform.position;
        unitBuildPos.Add(unit.transform.position); // 유닛 설치 위치 등록

        Invoke("BombUnit_TimeOver", unitLifeTime); // 유닛 유지시간 동안 대기 후 오브젝트 풀로 유닛 이동  
    }

    private void Unit_TimeOver() 
    {
        Debug.Log("유닛 유지시간 종료");
        unitBuildPos.Remove(unit.transform.position); // 유닛 설치 위치 제거
        unit.transform.position = poolPos;
    }
    #endregion

    
}
