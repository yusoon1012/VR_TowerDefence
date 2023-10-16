using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Blade : MonoBehaviour
{
    // Į��
    private GameObject blade = default;
    // Į�� ȸ�� ����
    private bool rotateBlade = false;

    private void Awake()
    {
        blade = transform.GetChild(0).gameObject;
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
            Debug.Log("�� ����!");
            rotateBlade = true;
        }
        else rotateBlade = false; // �ƴϸ� ����
    }

    private void FixedUpdate()
    {
        if (rotateBlade)
        {
            float rotateValue = 500f; // Į�� ȸ����
            blade.transform.RotateAround(transform.position, Vector3.up, rotateValue * Time.deltaTime); // ȸ��
        }
        else blade.transform.rotation = blade.transform.rotation; // ����
    }
}
