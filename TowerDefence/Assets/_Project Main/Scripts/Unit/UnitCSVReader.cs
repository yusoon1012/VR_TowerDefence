using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unit 속성을 담은 CSV 파일을 읽어온다
/// </summary>
public class UnitCSVReader : MonoBehaviour
{
    // 유닛 CSV TextAsset
    [Header("CSV File")]
    public TextAsset attackUnitCSV, buffUnitCSV = default;

    private void Awake()
    {
        ReadCSV();
    }

    /// <summary>
    /// 유닛 정보를 담은 CSV 파일을 읽어들임 
    /// </summary>
    private void ReadCSV()
    {
        // CSV Line
        string[] attackUnitCSV_Line, buffUnitCSV_Line = default;

        int power_Index = -1;
        int speed_Index = -1;
        int recognitionRange_Index = -1;
        int attackRange_Index = -1;

        if (attackUnitCSV != null && buffUnitCSV != null)
        {
            // 개행문자 분할
            attackUnitCSV_Line = attackUnitCSV.text.Split('\n');
            buffUnitCSV_Line = buffUnitCSV.text.Split('\n');

            // 개행문자 삭제 
            for (int i = 0; i < attackUnitCSV_Line.Length; i++)
            {
                attackUnitCSV_Line[i] = attackUnitCSV_Line[i].TrimEnd('\n');
            }
            for (int i = 0; i < buffUnitCSV_Line.Length; i++)
            {
                buffUnitCSV_Line[i] = attackUnitCSV_Line[i].TrimEnd('\n');
            }

            #region 설치/공격형 유닛
            string[] headers = attackUnitCSV_Line[0].Split(','); // 헤더
            string[] lines = default; // 속성 실 내용

            // 유닛 헤더 체크
            for (int i = 0; i < headers.Length; i++)
            {
                /// IF TODO: 만약 나중에 유닛 추가 시 "Type" 헤더도 추가할 것 
                if (headers[i] == "Power")
                {
                    power_Index = i;
                }
                else if (headers[i] == "Speed")
                {
                    speed_Index = i;
                }
                else if (headers[i] == "Recognition_Range")
                {
                    recognitionRange_Index = i;
                }
                /// Point: (나중에 수정 필요) Attack_Range 뒤 개행문자 탓인지 제대로 못잡아서 임시방편으로 처리
                else if (headers[i] == headers[4])
                {
                    attackRange_Index = i;
                }
            }

            // 속성 실부여
            foreach (GameObject unit in UnitBuildSystem.units) // 유닛 리스트 전체 검색
            {
                AttackUnitProperty attackComponent = unit.GetComponent<AttackUnitProperty>(); // 설치/공격형 유닛 확인
                UnitBuffSystem buffComponent = transform.GetComponent<UnitBuffSystem>(); // 버프형 유닛 확인

                if (attackComponent != null)
                {
                    if (unit.name == "Bomb Unit(Clone)") // 폭탄 유닛일때
                    {
                        lines = attackUnitCSV_Line[1].Split(','); // Bomb에 해당하는 줄
                        AttackUnitProperty bombProperty = unit.GetComponent<AttackUnitProperty>();

                        bombProperty.power = int.Parse(lines[power_Index]);
                        bombProperty.speed = int.Parse(lines[speed_Index]);
                        bombProperty.recognition_Range = int.Parse(lines[recognitionRange_Index]);
                        bombProperty.attack_Range = int.Parse(lines[attackRange_Index]);
                    }
                    else if (unit.name == "Blade Unit(Clone)") // 칼날 유닛일때
                    {
                        lines = attackUnitCSV_Line[2].Split(','); // Blade에 해당하는 줄
                        AttackUnitProperty bladeProperty = unit.GetComponent<AttackUnitProperty>();

                        bladeProperty.power = int.Parse(lines[power_Index]);
                        bladeProperty.speed = int.Parse(lines[speed_Index]);
                        bladeProperty.recognition_Range = int.Parse(lines[recognitionRange_Index]);
                        bladeProperty.attack_Range = int.Parse(lines[attackRange_Index]);
                    }
                }

                if (buffComponent != null)
                {

                }
            }
            #endregion
        }
        else Debug.Log("CSV 없음");
    }
}
