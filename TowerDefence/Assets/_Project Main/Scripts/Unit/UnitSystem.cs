#define PC
#define Oculus
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSystem : MonoBehaviour
{
    #region 게임 오브젝트 변수
    // 타워 프리팹
    [SerializeField] private GameObject bombUnitPrefab;
    [SerializeField] private GameObject bladeUnitPrefab;
    // 타워 오브젝트
    private GameObject bombUnit;
    private GameObject bladeUnit;
    // 선택 시의 유닛 프리팹
    [SerializeField] private GameObject selectBombPrefab;
    [SerializeField] private GameObject selectBladePrefab;
    // 선택 시의 유닛 오브젝트 
    private GameObject selectBombUnit;
    private GameObject selectBladeUnit;
    // 그리드
    [SerializeField] private Grid grid;
    // 플레이어
    [SerializeField]private GameObject player;
    #endregion

    // 풀 포지션
    private Vector3 poolPos = new Vector3(0f, -10f, 0f);

    private void Awake()
    {
        // 타워 생성 
        bombUnit = Instantiate(bombUnitPrefab, poolPos, Quaternion.identity);
        bladeUnit = Instantiate(bladeUnitPrefab, poolPos, Quaternion.identity);
        // 선택 시 타워 오브젝트 생성
        selectBombUnit = Instantiate(selectBombPrefab, poolPos, Quaternion.identity);
        selectBladeUnit = Instantiate(selectBladePrefab, poolPos, Quaternion.identity);
    }

    private void Update()
    {
#if PC
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Tower_Range(); // 설치 범위 표시

            if (ARAVRInput.Get(ARAVRInput.Button.Thumbstick))
            {
                Tower_Build(); // 타워 설치
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            selectBombUnit.transform.position = poolPos; // 손을 뗐다면 풀로 복귀 
        }

#elif Oculus
        if (ARAVRInput.Get(ARAVRInput.Button.Thumbstick))
        {
            Tower_Range();
        }

#endif
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

    /// <summary>
    /// 유닛 배치 범위를 표시
    /// </summary>
    private void Tower_Range()
    {
        Vector3 buildPos = SelectPosition; // 유닛을 설치할 좌표
        buildPos.y += 0.5f; // TODO: 선택유닛 콜라이더의 절반 높이 추가 => GetComponent<Collider>(), collider.bounds.size.y

        // TODO: if문 거리 제한 (플레이어, 설치 위치)
        // TODO: 유닛창 추가 후 유닛 선택 추가
        selectBombUnit.transform.position = buildPos;
    }

    /// <summary>
    /// 유닛을 배치
    /// </summary>
    private void Tower_Build()
    {
        // TODO: 유닛창 추가 후 유닛 선택 추가
        // 타워 설치 
        //bombUnit.transform.position = selectBombUnit.transform.position;
        bladeUnit.transform.position = selectBombUnit.transform.position;
    }
}
