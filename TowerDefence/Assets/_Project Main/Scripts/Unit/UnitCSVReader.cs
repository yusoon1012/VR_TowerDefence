using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unit �Ӽ��� ���� CSV ������ �о�´�
/// </summary>
public class UnitCSVReader : MonoBehaviour
{
    // ���� CSV TextAsset
    [Header("CSV File")]
    public TextAsset attackUnitCSV, buffUnitCSV = default;

    private void Awake()
    {
        ReadCSV();
    }

    /// <summary>
    /// ���� ������ ���� CSV ������ �о���� 
    /// </summary>
    private void ReadCSV()
    {
        string[] attackUnitCSV_Line, buffUnitCSV_Line = default;

        if (attackUnitCSV != null && buffUnitCSV != null)
        {
            // ���๮�� ����
            attackUnitCSV_Line = attackUnitCSV.text.Split('\n');
            buffUnitCSV_Line = buffUnitCSV.text.Split('\n');

            string[] headers = attackUnitCSV_Line[0].Split(','); // ���
            // ��ġ/������ ���� ��ǥ ����
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == "")
                {

                }
                //string[] attackValue = attackUnitCSV_Line[i].Split(',');
            }
            // ������ ���� ��ǥ ���� 
            for (int i = 1; i < buffUnitCSV_Line.Length; i++)
            {
                string[] buffValue = buffUnitCSV_Line[i].Split(',');
            }
        }
        else Debug.Log("CSV ����");
    }

    private void EndueProperty()
    {
        foreach (GameObject unit in UnitBuildSystem.units)
        {
            AttackUnitProperty attackComponent = unit.GetComponent<AttackUnitProperty>();

            if (attackComponent != null)
            {
                Debug.Log("���� ������ Ȯ����.");
            }
        }
    }

}
