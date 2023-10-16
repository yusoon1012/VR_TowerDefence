using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    #region �̱��� ����
    public static MonsterSpawn instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion // �̱��� ����

    [SerializeField]    // ���� �������� �� ����  *���� ���� �����Ǹ� �迭�� �ٲ� ����*
    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField]    // ������Ʈ Ǯ���� �������� �θ� ������Ʈ
    private Transform parent;

    [SerializeField]    // ���� ���� ����
    [Range(0.1f, 1.0f)]      // �ִ밪 ���� ����
    private float maxTime = 0.5f;

    [SerializeField]    // �÷��̾�� Normal ���� ������ �Ÿ� (�ּ� ��)
    private int minNormalMonsterDistance;
    [SerializeField]    // �÷��̾�� Normal ���� ������ �Ÿ� (�ִ� ��)
    private int maxNormalMonsterDistance;
    [SerializeField]    // �÷��̾�� fast ���� ������ �Ÿ� (�ּ� ��) 
    private int minFastMonsterDistance;
    [SerializeField]    // �÷��̾�� fast ���� ������ �Ÿ� (�ִ� ��) 
    private int maxFastMonsterDistance;

    [SerializeField]    // �÷��̾� ��ġ�� �˱� ���� ����
    private GameObject playerPos;
    [SerializeField]    // ���� ���� ��ġ�� �˱� ���� ����
    private GameObject bossPos;
    [SerializeField]
    private int indexMonsterCount = default;        // �ϳ��� �ε����� ��ȯ�� ����� ����

    [SerializeField]
    private LayerMask layerMask = default;          // �� ���̾� ����

    private int maxMonsterCount = default;          // �� ���̺�� ��ȯ�� ���� ����
    private float[] indexDegrees = new float[6];    // ���Ͱ� ��ȯ�� �ε��� ��
    private int currentCount = default;             // ������ ���� ����
    private bool isSpawn = false;                   // �ߺ� ��ȯ ������ ���� bool��

    private int[] indexSpawnChecks = default;      // �ε����� �ִ� ���� ���� ��ȯ ����� üũ

    private Vector3 raycastDown = new Vector3(0.0f, -1.0f, 0.0f);     // ���� ���� (�Ʒ�)
    private float raycastRange = 100.0f;             // ���� ����

    //! ���Ͱ� ��ȯ�� �ε��� �� �����ֱ�, indexSpawnChecks �迭 0���� �ʱ�ȭ
    private void Start()
    {
        SpawnIndex();

        maxMonsterCount = indexMonsterCount * 5;
        indexSpawnChecks = new int[5];

        for (int i = 0; i < indexSpawnChecks.Length; i++)
        {
            indexSpawnChecks[i] = 0;
        }
    }       // Start()

    //! ���� ���� �ٸ� ��ũ��Ʈ���� �����Ҷ� SetWave()�Լ� ���
    public void SetWave()
    {
        if (!isSpawn)
        {
            StartCoroutine(StartSpawn());
        }       // if: �ߺ� ��ȯ ����
    }       // SetWave()

    //! ���� ��ȯ
    private IEnumerator StartSpawn()
    {
        isSpawn = true;

        while (currentCount < maxMonsterCount)
        {
            if (maxMonsterCount < currentCount)
            {
                yield break;
            }       // if: Ȥ�ø� ����ó��

            SelectIndex();

            float duration = Random.Range(0, maxTime);

            yield return new WaitForSeconds(duration);

            currentCount += 1;

        }       // loop: wave �� ������ ���Ͱ� �ִ밪�� �����Ҷ����� �ݺ�

        // ��ȯ �ϸ鼭 �ʿ��� �� �ʱ�ȭ
        // TODO: �ۼ� �ؾ���

        isSpawn = false;
    }       // StartSpawn()

    //! ���� ��ġ ������ �ֱ�
    private void SelectIndex()
    {
        while (true)
        {
            int randNumber = Random.Range(0, indexSpawnChecks.Length);

            if (indexSpawnChecks[randNumber] == 5) { continue; }
            else
            {
                if (randNumber == 2)
                {
                    FindMonster(monsters[0], randNumber);

                    indexSpawnChecks[randNumber] += 1;

                    break;
                }       // FastMonster ����
                else
                {
                    FindMonster(monsters[1], randNumber);

                    indexSpawnChecks[randNumber] += 1;

                    break;
                }       // NormalMonster ����
            }
        }
    }       // SpawnPos()

    //! ������Ʈ Ǯ���� ���� ���� �̸��� ���� �θ� ������Ʈ ã��
    private void FindMonster(GameObject _monster, int _index)
    {
        // ������ ���� ��ġ �������ֱ�
        Vector3 spawnPosition = SpawnPos(_index);

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == _monster.name)
            {
                if (parent.GetChild(i).childCount != 0)
                {
                    for (int j = 0; j < parent.GetChild(i).childCount; j++)
                    {
                        if (!parent.GetChild(i).GetChild(j).gameObject.activeSelf)
                        {
                            GameObject newMonster = parent.GetChild(i).GetChild(j).gameObject;

                            newMonster.transform.position = spawnPosition;

                            newMonster.SetActive(true);

                            break;
                        }       // if: ��Ȱ��ȭ ������ ������Ʈ�� �߰��ϸ� Ȱ��ȭ �����ֱ�
                    }       // loop: ������ ��ġ���� ��Ȱ��ȭ ������ ������Ʈ ã��
                }       // if: ������Ʈ Ǯ�� ����

                // parent.GetChild(i).childCount �� 0�̰ų� �ڽ��� ��� Ȱ��ȭ ���¶�� ���� ����
                Spawn(_monster, spawnPosition, i);

                break;
            }       // if: ���� �̸��� �θ� ������Ʈ�� �̸��� ������ Ȯ��
        }       // loop: ������ ������Ʈ�� �θ������Ʈ ����
    }       // FindMonster()

    //! �����ϱ�
    private void Spawn(GameObject _monster, Vector3 _position, int _num)
    {
        // ���ο� ���� ����
        Instantiate(_monster, _position, Quaternion.identity, parent.GetChild(_num));

    }       // Spawn()

    //! �Ÿ� ���� ��ġ�� ã��
    private Vector3 SpawnPos(int _index)
    {
        Vector3 spawnPosition = Vector3.zero;

        float direction = Random.Range(indexDegrees[_index], indexDegrees[_index + 1]) - 180.0f;

        if (_index == 2)
        {
            Vector3 spawnDirection = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), 0.0f, Mathf.Sin(direction * Mathf.Deg2Rad));
            Vector3 spawnVectorXZ = (playerPos.transform.position - new Vector3(0.0f, playerPos.transform.position.y, 0.0f))
                + (spawnDirection * Random.Range(minFastMonsterDistance, maxFastMonsterDistance));

            spawnPosition = RaycastPos(spawnVectorXZ);
        }
        else
        {
            Vector3 spawnDirection = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), 0.0f, Mathf.Sin(direction * Mathf.Deg2Rad));
            Vector3 spawnVectorXZ = (playerPos.transform.position - new Vector3(0.0f, playerPos.transform.position.y, 0.0f))
                + (spawnDirection * Random.Range(minNormalMonsterDistance, maxNormalMonsterDistance));

            spawnPosition = RaycastPos(spawnVectorXZ);
        }

        return spawnPosition;
    }

    //! ����ĳ��Ʈ�� �̿��Ͽ� ���� ã��
    private Vector3 RaycastPos(Vector3 _vector)
    {
        _vector.y += 100.0f;        // ����ĳ��Ʈ�� ������ �Ʒ��� ��� ���� ����

        RaycastHit hit;

        if (Physics.Raycast(_vector, raycastDown, out hit, raycastRange, layerMask))
        {
            return hit.point;
        }       // ����ĳ��Ʈ �Ʒ��� ��ƿ÷��� ��
        else
        {
            Debug.Log("Find not Ground Layer");

            return hit.point;
        }
    }

    //! ���� ���� ���� �����ϱ�
    private void SpawnIndex()
    {
        // ���� ���Ϳ� �÷��̾� ������ ���� ���
        Vector3 directionTarget = bossPos.transform.position - playerPos.transform.position;
        float currentDegree = Mathf.Atan2(directionTarget.x, directionTarget.z) * Mathf.Rad2Deg;

        // -90 ~ +90 ���̸� 5��� �Ͽ� Index �����
        for (int i = 0; i < 6; i++)
        {
            // �÷��̾� ���� ������ �ٶ� �� �ʱ� ����: currentDegree
            // i ������ ���� �迭�� -90 ~ 90���� 6��� �ϱ�
            indexDegrees[i] = currentDegree - 90 + i * 36;
        }
    }
}       // class MonsterSpawn
