using System;
using System.Collections;
using System.Collections.Generic;
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
    private Animation animation;
    private GameObject player;
    private NavMeshAgent nav;
    private string monsterName;
    private float distance;

    [SerializeField]
    private GameObject attackFX;
    [SerializeField]
    private GameObject lifeFX;

    private bool isReady = false;

    private bool isAttack = false;
    private bool isBuffer = false;
    private bool isDeath = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        monsterName = transform.name.Replace("(Clone)", "");

        attackFX.SetActive(false);
        lifeFX.SetActive(false);

        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            animation = GetComponent<Animation>();
        }

        nav = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        MonsterReset();
        SetMonster();
    }       // OnEnable()

    private void Start()
    {
        MonsterReset();
        SetMonster();
    }       // Start()

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

    #region 에니메이션
    //! 몬스터의 공격 에니메이션 재생
    private IEnumerator Attack()
    {
        isAttack = true;

        if (animator != null)
        {
            animator.SetTrigger("Attack");

            if (!AniCheckInfo("Attack"))
            {
                while (!AniCheckInfo("Attack"))
                {
                    yield return null;
                }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
            }       // if: 에니메이션이 잘 들어왔을 경우 패스

            float timeElpased = 0.0f;
            float duration = AniCheckLength();

            //! TODO: 거리를 계산하여 거리 내에 있는 오브젝트 체력 닳게 하기 또는 콜라이더 생성후 Trigger 닿은 몬스터 체력 닳게 하기

            attackFX.SetActive(true);

            while (timeElpased < duration)
            {
                timeElpased += Time.deltaTime;

                // TODO: 디졸브 효과 보간으로 추가

                yield return null;
            }
        }       // Animator 기반
        else if (animation != null)
        {
            animation.Play("Attack");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Attack").length;

            attackFX.SetActive(true);

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                // TODO: 디졸브 효과 보간으로 추가

                yield return null;
            }
        }       // Animation 기반

        this.gameObject.SetActive(false);
    }       // AttackPlayer()       // 사운드, 디졸브, 효과 추가 예정

    //! 몬스터의 능력 향상 에니메이션 재생
    private IEnumerator Buffer()
    {
        isBuffer = true;

        if (animator != null)
        {
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
        }
        else if (animation != null)
        {
            animation.Play("Buffer");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Buffer").length;

            // TODO: 사운드, 효과

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // TODO: 능력

            animation.Play("Walk");
        }

        isBuffer = false;
    }       // Buffer()     // 사운드, 효과 추가 예정

    //! 몬스터의 죽어을 때 에니메이션 재생
    private IEnumerator Death()
    {
        isDeath = true;

        if (animator != null)
        {
            animator.SetTrigger("Death");

            if (!AniCheckInfo("Death"))
            {
                while (!AniCheckInfo("Death"))
                {
                    yield return null;
                }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
            }       // if: 에니메이션이 잘 들어왔을 경우 패스

            float timeElpased = 0.0f;
            float duration = AniCheckLength();

            // TODO: 사운드, 효과

            while (timeElpased < duration)
            {
                timeElpased += Time.deltaTime;

                // TODO: 디졸브 효과 보간으로 추가

                yield return null;
            }
        }
        else if (animation != null)
        {
            animation.Play("Death");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Death").length;

            // TODO: 사운드, 효과

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                // TODO: 디졸브 효과 보간으로 추가

                yield return null;
            }
        }

        this.gameObject.SetActive(false);
    }       // Death()      // 사운드, 디졸브, 효과 추가 예정
    #endregion

    #region 초기화
    //! 오브젝트 풀링을 위한 리셋
    private void MonsterReset()
    {
        isReady = false;
        isAttack = false;
        isBuffer = false;
        isDeath = false;

        attackFX.SetActive(false);
        lifeFX.SetActive(false);

        transform.LookAt(player.transform);

        SetMonster();

        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }

        lifeFX.SetActive(true);

        if (animation != null) { animation.Play("Walk"); }
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
