using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    #region 싱글턴 선언
    public static MonsterSpawn instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion // 싱글턴 선언

    [SerializeField]    // 몬스터 프리펩이 들어갈 공간  *몬스터 종류 결정되면 배열로 바뀔 예정*
    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField]    // 오브젝트 풀링을 쓰기위한 부모 오브젝트
    private Transform parent;

    [SerializeField]    // 몬스터 스폰 간격
    [Range(0.1f, 1.0f)]      // 최대값 한정 범위
    private float maxTime = 0.5f;

    [SerializeField]    // 플레이어와 Normal 몬스터 사이의 거리 (최소 값)
    private int minNormalMonsterDistance;
    [SerializeField]    // 플레이어와 Normal 몬스터 사이의 거리 (최대 값)
    private int maxNormalMonsterDistance;
    [SerializeField]    // 플레이어와 fast 몬스터 사이의 거리 (최소 값) 
    private int minFastMonsterDistance;
    [SerializeField]    // 플레이어와 fast 몬스터 사이의 거리 (최대 값) 
    private int maxFastMonsterDistance;

    [SerializeField]    // 플레이어 위치를 알기 위한 변수
    private GameObject playerPos;
    [SerializeField]    // 보스 몬스터 위치를 알기 위한 변수
    private GameObject bossPos;
    [SerializeField]
    private int indexMonsterCount = default;                      // 하나의 인덱스에 소환될 몬수터 갯수

    [SerializeField]
    private LayerMask layerMask = default;                        // 땅 레이어 지정

    private int maxMonsterCount = default;                        // 한 웨이브당 소환될 몬스터 갯수
    private float[] indexDegrees = new float[6];                  // 몬스터가 소환될 인덱스 값
    public int currentCount = default;                           // 스폰한 몬스터 갯수
    private bool isSpawn = false;                                 // 중복 소환 방지를 위한 bool값

    private int[] indexSpawnChecks = default;                     // 인덱스에 최대 마리 수가 소환 됬는지 체크

    private Vector3 raycastDown = new Vector3(0.0f, -1.0f, 0.0f); // 레이 방향 (아래)
    private float raycastRange = 100.0f;                          // 레이 길이

    //! 몬스터가 소환될 인덱스 값 정해주기, indexSpawnChecks 배열 0으로 초기화
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

    //! 스폰 시작 다른 스크립트에서 스폰할때 SetWave()함수 사용
    public void SetWave()
    {
        if (!isSpawn)
        {
            StartCoroutine(StartSpawn());
        }       // if: 중복 소환 방지
    }       // SetWave()

    //! 몬스터 소환
    private IEnumerator StartSpawn()
    {
        isSpawn = true;

        while (currentCount < maxMonsterCount)
        {
            if (maxMonsterCount < currentCount)
            {
                yield break;
            }       // if: 혹시모를 예외처리

            SelectIndex();

            float duration = Random.Range(0, maxTime);

            yield return new WaitForSeconds(duration);

            currentCount += 1;

        }       // loop: wave 당 스폰한 몬스터가 최대값에 도달할때까지 반복

        // 소환 하면서 필요한 값 초기화
        // TODO: 작성 해야함

        isSpawn = false;
    }       // StartSpawn()

    //! 스폰 위치 지정해 주기
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
                }       // FastMonster 선택
                else
                {
                    FindMonster(monsters[1], randNumber);

                    indexSpawnChecks[randNumber] += 1;

                    break;
                }       // NormalMonster 선택
            }
        }
    }       // SpawnPos()

    //! 오브젝트 풀링을 위한 몬스터 이름과 같은 부모 오브젝트 찾기
    private void FindMonster(GameObject _monster, int _index)
    {
        // 스폰될 몬스터 위치 선정해주기
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
                        }       // if: 비활성화 상태의 오브젝트를 발견하면 활성화 시켜주기
                    }       // loop: 스폰할 위치에서 비활성화 상태인 오브젝트 찾기
                }       // if: 오브젝트 풀링 시작

                // parent.GetChild(i).childCount 가 0이거나 자식이 모두 활성화 상태라면 새로 스폰
                Spawn(_monster, spawnPosition, i);

                break;
            }       // if: 몬스터 이름과 부모 오브젝트의 이름이 같은지 확인
        }       // loop: 스폰될 오브젝트의 부모오브젝트 선정
    }       // FindMonster()

    //! 스폰하기
    private void Spawn(GameObject _monster, Vector3 _position, int _num)
    {
        // 새로운 몬스터 생성
        Instantiate(_monster, _position, Quaternion.identity, parent.GetChild(_num));
    }       // Spawn()

    //! 거리 랜던 위치값 찾기
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

    //! 레이캐스트를 이용하여 지면 찾기
    private Vector3 RaycastPos(Vector3 _vector)
    {
        _vector.y += 100.0f;        // 레이캐스트를 위에서 아래로 쏘기 위한 로직

        RaycastHit hit;

        if (Physics.Raycast(_vector, raycastDown, out hit, raycastRange, layerMask))
        {
            return hit.point;
        }       // 레이캐스트 아래로 쏘아올렸을 때
        else
        {
            Debug.Log("Find not Ground Layer");

            return hit.point;
        }
    }

    //! 몬스터 스폰 구간 지정하기
    private void SpawnIndex()
    {
        // 보스 몬스터와 플레이어 사이의 각도 계산
        Vector3 directionTarget = bossPos.transform.position - playerPos.transform.position;
        float currentDegree = Mathf.Atan2(directionTarget.x, directionTarget.z) * Mathf.Rad2Deg;

        // -90 ~ +90 사이를 5등분 하여 Index 만들기
        for (int i = 0; i < 6; i++)
        {
            // 플레이어 기준 보스를 바라볼 때 초기 각도: currentDegree
            // i 증가에 따라 배열에 -90 ~ 90으로 6등분 하기
            indexDegrees[i] = currentDegree - 90 + i * 36;
        }
    }

    //! 미니맵에 표시할 몬스터 스폰
    private void MinimapSpawn()
    { 
    
    }
}       // class MonsterSpawn
