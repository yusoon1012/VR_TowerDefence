#define PC
#define Oculus
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSystem : MonoBehaviour
{
    #region ���� ������Ʈ ����
    // Ÿ�� ������
    [SerializeField] private GameObject bombUnitPrefab;
    [SerializeField] private GameObject bladeUnitPrefab;
    // Ÿ�� ������Ʈ
    private GameObject bombUnit;
    private GameObject bladeUnit;
    // ���� ���� ���� ������
    [SerializeField] private GameObject selectBombPrefab;
    [SerializeField] private GameObject selectBladePrefab;
    // ���� ���� ���� ������Ʈ 
    private GameObject selectBombUnit;
    private GameObject selectBladeUnit;
    // �׸���
    [SerializeField] private Grid grid;
    // �÷��̾�
    [SerializeField]private GameObject player;
    #endregion

    // Ǯ ������
    private Vector3 poolPos = new Vector3(0f, -10f, 0f);

    private void Awake()
    {
        // Ÿ�� ���� 
        bombUnit = Instantiate(bombUnitPrefab, poolPos, Quaternion.identity);
        bladeUnit = Instantiate(bladeUnitPrefab, poolPos, Quaternion.identity);
        // ���� �� Ÿ�� ������Ʈ ����
        selectBombUnit = Instantiate(selectBombPrefab, poolPos, Quaternion.identity);
        selectBladeUnit = Instantiate(selectBladePrefab, poolPos, Quaternion.identity);
    }

    private void Update()
    {
#if PC
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Tower_Range(); // ��ġ ���� ǥ��

            if (ARAVRInput.Get(ARAVRInput.Button.Thumbstick))
            {
                Tower_Build(); // Ÿ�� ��ġ
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            selectBombUnit.transform.position = poolPos; // ���� �ôٸ� Ǯ�� ���� 
        }

#elif Oculus
        if (ARAVRInput.Get(ARAVRInput.Button.Thumbstick))
        {
            Tower_Range();
        }

#endif
    }

    /// <summary>
    /// ���� ��ġ�� ���� ��ǥ�� ����
    /// </summary>
    private Vector3 SelectPosition
    {
        get
        {
            Vector3 selectPos = default;
            int floorLayer = 1 << LayerMask.NameToLayer("Terrain");
            // Ray�� ī�޶��� ��ġ�κ��� �������� �Ѵ�.
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection); // �� �ȵǳ�...?
            RaycastHit hitInfo = default;

            if (Physics.Raycast(ray, out hitInfo, 200f, floorLayer))
            {
                selectPos = hitInfo.point;
            }

            Vector3Int gridPos = grid.WorldToCell(selectPos);

            return gridPos; // ���� ���������� return �ϵ��� ����
        }
    }

    /// <summary>
    /// ���� ��ġ ������ ǥ��
    /// </summary>
    private void Tower_Range()
    {
        Vector3 buildPos = SelectPosition; // ������ ��ġ�� ��ǥ
        buildPos.y += 0.5f; // TODO: �������� �ݶ��̴��� ���� ���� �߰� => GetComponent<Collider>(), collider.bounds.size.y

        // TODO: if�� �Ÿ� ���� (�÷��̾�, ��ġ ��ġ)
        // TODO: ����â �߰� �� ���� ���� �߰�
        selectBombUnit.transform.position = buildPos;
    }

    /// <summary>
    /// ������ ��ġ
    /// </summary>
    private void Tower_Build()
    {
        // TODO: ����â �߰� �� ���� ���� �߰�
        // Ÿ�� ��ġ 
        //bombUnit.transform.position = selectBombUnit.transform.position;
        bladeUnit.transform.position = selectBombUnit.transform.position;
    }
}
