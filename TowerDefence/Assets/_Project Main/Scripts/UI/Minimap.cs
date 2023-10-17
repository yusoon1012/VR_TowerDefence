using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    private Transform parent = default;

    #region 플레이어 변수
    [SerializeField]
    private Transform playerReal = default;
    [SerializeField]
    private RectTransform playerMap = default;
    #endregion

    #region 보스 변수
    [SerializeField]
    private Transform bossReal = default;
    [SerializeField]
    private Transform bossMapParent = default;
    [SerializeField]
    private GameObject bossMap = default;
    #endregion

    #region 몬스터 변수
    [SerializeField]
    private Transform monsterReal = default;
    [SerializeField]
    private GameObject monsterMap = default;
    #endregion


    private float currentDegree = default;

    private List<Vector3> monsters = new List<Vector3>();

    #region 식별기 라인 변수
    [SerializeField]
    private GameObject checkLine = default;
    [SerializeField]
    private float duration = 3.0f;

    private Color zeroColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private Color fillColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Vector2 startPos = new Vector2(0.0f, -300.0f);
    private Vector2 endPos = new Vector2(0.0f, 300.0f);
    #endregion

    private void Start()
    {
        //Instantiate(bossMap, bossReal.transform.position, Quaternion.identity, bossMapParent);

        StartCoroutine(CheckLine());
    }

    private void Update()
    {
        playerMap.eulerAngles = new Vector3(0.0f, 0.0f, -playerReal.rotation.eulerAngles.y);
    }

    public void SetMonster(Vector3 _vector)
    {

    }

    #region 식별기 라인 코루틴
    private IEnumerator CheckLine()
    {
        // TODO: 플레이어가 죽으면 코루틴 종료시점 만들기

        while (true)
        {
            StartCoroutine(CheckLineColor(duration * 0.1f, zeroColor, fillColor));
            StartCoroutine(CheckLinePos());

            yield return new WaitForSeconds(duration * 0.9f);

            StartCoroutine(CheckLineColor(duration * 0.1f, fillColor, zeroColor));

            yield return new WaitForSeconds(duration * 0.1f);
        }
    }

    private IEnumerator CheckLineColor(float _duration, Color _zeroColor, Color _fillColor)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / _duration);

            checkLine.GetComponent<Image>().color = Color.Lerp(_zeroColor, _fillColor, time);

            yield return null;
        }

        checkLine.GetComponent<Image>().color = _fillColor;
    }

    private IEnumerator CheckLinePos()
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            checkLine.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, time);

            yield return null;
        }

        checkLine.GetComponent<RectTransform>().anchoredPosition = endPos;
    }
    #endregion
}
