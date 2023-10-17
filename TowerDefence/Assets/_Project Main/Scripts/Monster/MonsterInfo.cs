using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 * 2023 - 10 - 15
 * 몬스터 애니메이션에 맞게 효과 수정
 * 멍멍이 초기 애니메이션 걷기 오류
 * 멍멍이 공격 할 때 효과 오류
 * 몬스터 레이 오류
 * 사운드 찾기
 */

public class MonsterInfo : MonsterData
{
    private Animator animator;
    private GameObject player;
    private NavMeshAgent nav;
    private string monsterName;
    private float distance;

    private Material material;
    [SerializeField]
    private GameObject attackFX;
    [SerializeField]
    private GameObject lifeFX;

    private bool isReady = false;

    private bool isAttack = false;
    private bool isBuffer = false;
    private bool isDeath = false;

    private float splitStartValue = -0.1f;
    private float splitEndValue = 1.1f;
    private float lifeDuration = 6.0f;

    private void Awake()
    {
        FindComponet(transform);
        player = GameObject.FindWithTag("Player");
        monsterName = transform.name.Replace("(Clone)", "");

        attackFX.SetActive(false);
        lifeFX.SetActive(false);

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        SetMonster();
        MonsterReset();
    }       // OnEnable()

    private void Update()
    {
        // TODO: 플레이어 체력 0초과인지 체크하는 if문 작성 -------------------------------------------
        // 플레이어와 몬스터간의 거리 계산
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (isReady)
        {
            if (this.hp <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                if (distance <= this.attackRange && isAttack == false)
                {
                    nav.speed = 0.0f;

                    StartCoroutine(Attack());
                }
                else
                {
                    nav.speed = this.speed;

                    TargetPlayer();
                }
            }
        }
    }

    //! 몬스터가 플레이어를 찾아가는 네비게이션
    private void TargetPlayer()
    {
        if (distance < this.recognitionRange)
        {
            nav.SetDestination(player.transform.position);
        }
    }       // TargetPlayer()

    //! 디졸브 효과 코루틴
    private IEnumerator SetDissolve(float _duration, float _startValue, float _endValue)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / _duration);

            material.SetFloat("_SplitValue", Mathf.Lerp(_startValue, _endValue, time));

            yield return null;
        }

        material.SetFloat("_SplitValue", _endValue);
    }

    //! SkinnedMeshRenderer 컴포넌트를 찾는 재귀함수
    private void FindComponet(Transform _parent)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            if (_parent.GetChild(i).GetComponent<SkinnedMeshRenderer>())
            {
                material = _parent.GetChild(i).GetComponent<SkinnedMeshRenderer>().material;

                return;
            }
            else
            {
                FindComponet(_parent.GetChild(i));
            }
        }
    }

    #region 에니메이션
    //! 몬스터의 공격 에니메이션 재생
    private IEnumerator Attack()
    {
        isAttack = true;

        animator.SetTrigger("Attack");

        if (!AniCheckInfo("Attack"))
        {
            while (!AniCheckInfo("Attack"))
            {
                yield return null;
            }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
        }       // if: 에니메이션이 잘 들어왔을 경우 패스

        float duration = AniCheckLength();

        //! TODO: 거리를 계산하여 거리 내에 있는 오브젝트 체력 닳게 하기 또는 콜라이더 생성후 Trigger 닿은 몬스터 체력 닳게 하기

        attackFX.SetActive(true);

        StartCoroutine(SetDissolve(duration, splitEndValue, splitStartValue));

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }       // AttackPlayer()       // 사운드 추가 예정

    //! 몬스터의 능력 향상 에니메이션 재생
    private IEnumerator Buffer()
    {
        isBuffer = true;

        animator.SetTrigger("Buffer");

        if (!AniCheckInfo("Buffer"))
        {
            while (!AniCheckInfo("Buffer"))
            {
                yield return null;
            }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
        }       // if: 에니메이션이 잘 들어왔을 경우 패스

        // TODO: 사운드, 효과

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Buffer"))
        {
            yield return null;
        }

        // TODO: 능력

        isBuffer = false;
    }       // Buffer()     // 사운드, 효과, 능력 추가 예정

    //! 몬스터의 죽어을 때 에니메이션 재생
    private IEnumerator Death()
    {
        isDeath = true;

        animator.SetTrigger("Death");

        if (!AniCheckInfo("Death"))
        {
            while (!AniCheckInfo("Death"))
            {
                yield return null;
            }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
        }       // if: 에니메이션이 잘 들어왔을 경우 패스

        float duration = AniCheckLength();

        // TODO: 사운드

        StartCoroutine(SetDissolve(duration, splitEndValue, splitStartValue));

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }       // Death()      // 사운드 추가 예정
    #endregion

    #region 초기화
    //! 오브젝트 풀링을 위한 리셋
    private void MonsterReset()
    {
        isReady = false;
        isAttack = false;
        isBuffer = false;
        isDeath = false;

        material.SetFloat("_SplitValue", splitStartValue);

        attackFX.SetActive(false);
        lifeFX.SetActive(false);

        transform.LookAt(player.transform);

        SetMonster();

        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }

        StartCoroutine(SetDissolve(lifeDuration, splitStartValue, splitEndValue));

        lifeFX.SetActive(true);

        isReady = true;
    }
    #endregion

    #region 에니메이션 정보
    //! 현재 진행되고 있는 에니메이션 체크
    private bool AniCheckInfo(string _name)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //! 현재 진행되고 있는 에니메이션 클립 길이
    private float AniCheckLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    #endregion

    #region 몬스터 속성값 정해주기
    //! 몬스터의 정보 정해 주기
    private void SetMonster()
    {
        switch (monsterName)
        {
            case "NormalMonster":

                (hp, power, speed, recognitionRange, attackRange, explosionRange) = NormalMonster();

                break;
            case "FastMonster":

                (hp, power, speed, recognitionRange, attackRange, explosionRange) = FastMonster();

                break;
        }
    }       // SetMonster()

    protected virtual (int, int, int, int, int, int) NormalMonster()
    {
        return base.NormalMonster(hp, power, speed, recognitionRange, attackRange, explosionRange);
    }

    protected virtual (int, int, int, int, int, int) FastMonster()
    {
        return base.FastMonster(hp, power, speed, recognitionRange, attackRange, explosionRange);
    }
    #endregion
}
