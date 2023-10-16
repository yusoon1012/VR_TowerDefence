using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// ��ġ/������ ����: ���� ���� �� ����
/// </summary>
public class UnitAttack_Bomb : MonoBehaviour
{
    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
    }

    /// <summary>
    /// ������ �����ϴ� �޼���
    /// </summary>
    private Collider[] EnemyGotcha
   {
        get
        {
            float radius = 5f; // ���� Ž�� �ݰ�
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // ���� ���̾��ũ
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer); // ���� ����

            return colliders;
        }
   }

    private void Update()
    {
        if (EnemyGotcha.Length > 0) // �ݰ� �� ���� �ϳ� �̻� ����
        {
            // TODO: ���� & ����Ʈ �߰� �� ���� ó�� ¥��
            Debug.Log("����!");
        }
    }
}
