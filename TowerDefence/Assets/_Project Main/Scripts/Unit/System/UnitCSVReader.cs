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

    private void Start()
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

        int attackRange_Index = -1; // 공격범위
        int damage_Index = -1; // 데미지(공격력)
        int attackCount_Index = -1; // 공격대상
        int speed_Index = -1; // 공격속도
        int rotateSpeed_Index = -1; // 회전속도
        int HP_Index = -1; // 체력

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
                // 공격범위, 데미지, 공격대상, 공격속도, 회전속도, 체력
                if (headers[i] == "공격범위")
                {
                    attackRange_Index = i;
                }
                else if (headers[i] == "데미지")
                {
                    damage_Index = i;
                }
                else if (headers[i] == "공격대상")
                {
                    attackCount_Index = i;
                }
                else if (headers[i] == "공격속도")
                {
                    speed_Index = i;
                }
                else if (headers[i] == "회전속도")
                {
                    rotateSpeed_Index = i;
                }
                else if (headers[i] == headers[8]) // Issue: 왜 '체력'을 체크하지 못하는가? 
                {
                    HP_Index = i;
                }
            }

            // 속성 실부여
            for (int i = 0; i < UnitBuildSystem.units.Count; i++) // 유닛 리스트 전체 검색
            {
                GameObject unit = UnitBuildSystem.units[i];
                Debug.LogFormat("유닛 개수: {0}", UnitBuildSystem.units.Count);
                AttackUnitProperty attackComponent = unit.GetComponent<AttackUnitProperty>(); // 설치/공격형 유닛 확인

                if (attackComponent != null)
                {
                    if (unit.name == "Unit_Bomb(Clone)") // 폭탄 유닛일때
                    {
                        lines = attackUnitCSV_Line[1].Split(','); // Bomb에 해당하는 줄
                        AttackUnitProperty bombProperty = unit.GetComponent<AttackUnitProperty>();

                        bombProperty.attackRange = int.Parse(lines[attackRange_Index]);
                        bombProperty.damage = int.Parse(lines[damage_Index]);
                        bombProperty.attackCount = int.Parse(lines[attackCount_Index]);
                        bombProperty.speed = float.Parse(lines[speed_Index]);
                        bombProperty.rotateSpeed = int.Parse(lines[rotateSpeed_Index]);
                        bombProperty.HP = int.Parse(lines[HP_Index]);
                    }
                    else if (unit.name == "Unit_Blade(Clone)") // 근거리 타격 유닛일때
                    {
                        lines = attackUnitCSV_Line[2].Split(','); // Blade에 해당하는 줄
                        AttackUnitProperty bladeProperty = unit.GetComponent<AttackUnitProperty>();

                        bladeProperty.attackRange = int.Parse(lines[attackRange_Index]);
                        bladeProperty.damage = int.Parse(lines[damage_Index]);
                        bladeProperty.attackCount = int.Parse(lines[attackCount_Index]);
                        bladeProperty.speed = float.Parse(lines[speed_Index]);
                        bladeProperty.rotateSpeed = int.Parse(lines[rotateSpeed_Index]);
                        bladeProperty.HP = int.Parse(lines[HP_Index]);

                    }
                    else if (unit.name == "Unit_ShootBoss(Clone)") // 보스 타격 유닛일때
                    {
                        lines = attackUnitCSV_Line[3].Split(','); // Blade에 해당하는 줄
                        AttackUnitProperty shootBossProperty = unit.GetComponent<AttackUnitProperty>();

                        shootBossProperty.attackRange = int.Parse(lines[attackRange_Index]);
                        shootBossProperty.damage = int.Parse(lines[damage_Index]);
                        shootBossProperty.attackCount = int.Parse(lines[attackCount_Index]);
                        shootBossProperty.speed = float.Parse(lines[speed_Index]);
                        shootBossProperty.rotateSpeed = int.Parse(lines[rotateSpeed_Index]);
                        shootBossProperty.HP = int.Parse(lines[HP_Index]);
                    }
                }
            }
            #endregion
        }
        else Debug.Log("CSV 없음");
    }
}
