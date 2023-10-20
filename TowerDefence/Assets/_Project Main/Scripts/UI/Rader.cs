using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rader : MonoBehaviour
{
    #region 플레이어 변수
    [SerializeField]
    private Transform playerReal = default;
    [SerializeField]
    private Transform playerRader = default;
    #endregion

    #region 보스 변수
    [SerializeField]
    private Transform bossReal = default;
    [SerializeField]
    private GameObject bossRader = default;
    #endregion

    #region 몬스터 변수
    [SerializeField]
    private Transform monsterRaderParent = default;
    [SerializeField]
    private GameObject monsterRader = default;

    private List<GameObject> monsterReals = new List<GameObject>();
    private List<GameObject> monsterRaders = new List<GameObject>();
    #endregion

    private float currentDegree = default;

    private void Start()
    {
        Vector3 directionTarget = bossReal.transform.position - playerReal.transform.position;
        currentDegree = Mathf.Atan2(directionTarget.x, directionTarget.z) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        playerRader.localEulerAngles = new Vector3(-90.0f, 0.0f, -playerReal.rotation.eulerAngles.y + currentDegree);

        for (int i = 0; i < monsterRaders.Count; i++)
        {
            monsterRaders[i].SetActive(monsterReals[i].activeSelf);

            Vector3 offset = new Vector3(playerReal.position.x, playerReal.position.z, 0.0f) -
                             new Vector3(monsterReals[i].transform.position.x, monsterReals[i].transform.position.z, 0.0f);

            // 현재 회전된 각도를 적용하여 setPos를 회전시킴
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x - 90.0f, transform.eulerAngles.y, transform.eulerAngles.z + currentDegree);

            Vector3 rotatedPos = rotation * -offset;

            // 회전한 위치를 사용하여 monsterRader의 위치를 설정
            monsterRaders[i].transform.position = rotatedPos * 0.008f + transform.position;
        }
    }

    public void SetMonster(GameObject _monster)
    {
        if (monsterRaders.Count != 0)
        {
            for (int i = 0; i < monsterRaders.Count; i++)
            {
                if (i == monsterRaders.Count - 1)
                {
                    monsterReals.Add(_monster);

                    GameObject newMonsterRader = Instantiate(monsterRader, monsterRaderParent);

                    monsterRaders.Add(newMonsterRader);

                    break;
                }
                else if (!monsterRaders[i].activeSelf)
                {
                    monsterReals[i] = _monster;

                    break;
                }
            }
        }
        else
        {
            monsterReals.Add(_monster);

            GameObject newMonsterRader = Instantiate(monsterRader, monsterRaderParent);

            monsterRaders.Add(newMonsterRader);
        }




        //monsterReals.Add(_monster);

        //GameObject newMonsterRader = Instantiate(monsterRader, monsterRaderParent);

        //monsterRaders.Add(newMonsterRader);
    }
}
