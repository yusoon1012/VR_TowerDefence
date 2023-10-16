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
        string[] attackUnitCSV_Line, buffUnitCSV_Line = default;

        if (attackUnitCSV != null && buffUnitCSV != null)
        {
            // 개행문자 분할
            attackUnitCSV_Line = attackUnitCSV.text.Split('\n');
            buffUnitCSV_Line = buffUnitCSV.text.Split('\n');

            #region 설치/공격형 유닛
            string[] headers = attackUnitCSV_Line[0].Split(','); // 헤더
            // 유닛 헤더 체크
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == "Power")
                {
                    int powerIndex = i;
                }
                else if (headers[i] == "Speed")
                {
                    int speedIndex = i;
                }
                else if (headers[i] == "Recognition_Range")
                {
                    int recognitionRangeIndex = i;
                }
                else if (headers[i] == "Attack_Range")
                {
                    int attackRangeIndex = i;
                }
            }
            // 속성 실부여
            for (int i = 1; i < attackUnitCSV_Line.Length; i++)
            {

            }
            #endregion

            #region 버프형 유닛
            //headers = buffUnitCSV_Line[0].Split(',');
            //// 설치/공격형 유닛 헤더 체크
            //for (int i = 0; i < headers.Length; i++)
            //{
            //    if (headers[i] == "Power")
            //    {
            //        int powerIndex = i;
            //    }
            //}
            #endregion
        }
        else Debug.Log("CSV 없음");
    }

    private void EndueProperty()
    {
        foreach (GameObject unit in UnitBuildSystem.units)
        {
            AttackUnitProperty attackComponent = unit.GetComponent<AttackUnitProperty>();

            if (attackComponent != null)
            {
                Debug.Log("공격 유닛을 확인함.");
            }
        }
    }

}
