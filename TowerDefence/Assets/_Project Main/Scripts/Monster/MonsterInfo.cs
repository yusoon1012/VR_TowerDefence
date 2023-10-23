using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private GameObject clearFX;
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> clip;

    private bool isReady = false;

    private bool isAttack = false;
    private bool isDeath = false;
    private bool isClear = false;

    private float splitStartValue = -0.1f;
    private float splitEndValue = 1.1f;
    private float lifeDuration = 6.0f;

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Awake()
    {
        FindComponet(transform);
        player = GameObject.FindWithTag("Player");
        monsterName = transform.name.Replace("(Clone)", "");

        attackFX.SetActive(false);
        lifeFX.SetActive(false);
        clearFX.SetActive(false);

        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        SetComponet(clearFX.transform);
        SetParticleColor();
    }

    private void OnEnable()
    {
        SetMonster();
        MonsterReset();
    }       // OnEnable()

    private void Update()
    {
        if (!isClear)   // 게임매니저의 클리어 조건 넣기
        {
            if (!isClear)
            {
               
               
                StartCoroutine(Clear());
            }

            return;
        }
        else
        {
            // 플레이어와 몬스터간의 거리 계산
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (isReady)
            {
                if (this.hp <= 0)
                {
                    StartCoroutine(Death(false));
                }
                else
                {
                    if (distance <= this.attackRange && isAttack == false)
                    {
                        nav.speed = 0.0f;

                        StartCoroutine(Attack());
                    }
                    else if (isAttack == false)
                    {
                        nav.speed = this.speed;

                        TargetPlayer();
                    }
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

    //! ParticleSystem 컴포넌트를 설정하는 재귀함수
    private void SetComponet(Transform _parent)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            if (_parent.GetChild(i).GetComponent<ParticleSystem>())
            {
                particleSystems.Add(_parent.GetChild(i).GetComponent<ParticleSystem>());
            }
        }
    }

    //! 몬스터 체력을 깍기
    public void MonsterDamaged(int _damage)
    {
        this.hp -= _damage;
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

        SoundManager(true);

        float duration = AniCheckLength();

        yield return new WaitForSeconds(duration / 2);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Player_Status.Instance.PlayerDamaged(power);
        }

        yield return new WaitForSeconds(duration / 2);

        isAttack = false;
    }       // AttackPlayer()

    //! 몬스터의 죽어을 때 에니메이션 재생
    private IEnumerator Death(bool _isClear)
    {
        isDeath = true;
        
                
        GoldManager.instance.EnemyDropGold();
                

        animator.SetTrigger("Death");

        if (!AniCheckInfo("Death"))
        {
            while (!AniCheckInfo("Death"))
            {
                yield return null;
            }       // loop: 에니메이션이 JumpStart에 들어올때까지 대기
        }       // if: 에니메이션이 잘 들어왔을 경우 패스

        float duration = AniCheckLength();

        SoundManager(false);

        if (monsterName == "NormalUpgradeMonster" && !isClear)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= explosionRange)
            {
                attackFX.SetActive(true);

                Player_Status.Instance.PlayerDamaged(power);
            }
        }

        StartCoroutine(SetDissolve(duration, splitEndValue, splitStartValue));

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }       // Death()
    #endregion

    #region 사운드
    private void SoundManager(bool _isAttack)
    {
        switch (_isAttack)
        {
            case true:

                if (clip.Count == 0) { break; }
                else { audioSource.clip = clip[0]; }

                break;
            case false:

                if (clip.Count == 1) { break; }
                else { audioSource.clip = clip[1]; }

                break;
        }

        audioSource.Play();
    }
    #endregion

    #region 초기화
    //! 오브젝트 풀링을 위한 리셋
    private void MonsterReset()
    {
        isReady = false;
        isAttack = false;
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

    #region 게임 종료 Event 관련
    //! 파티클 색상 선정
    private void SetParticleColor()
    {
        Color color = default;

        int randColor = Random.Range(0, 7);

        Debug.Log(randColor);

        switch (randColor)
        {
            case 0:

                color = new Color(1.0f, 0.0f, 0.0f);

                break;
            case 1:

                color = new Color(0.0f, 1.0f, 0.0f);

                break;
            case 2:

                color = new Color(0.0f, 0.0f, 1.0f);

                break;
            case 3:

                color = new Color(1.0f, 1.0f, 0.0f);

                break;
            case 4:

                color = new Color(1.0f, 0.0f, 1.0f);

                break;
            case 5:

                color = new Color(0.0f, 1.0f, 1.0f);

                break;
            case 6:

                color = new Color(1.0f, 1.0f, 1.0f);

                break;

        }

        for (int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].startColor = color;
        }
    }

    private IEnumerator Clear()
    {
        isClear = true;

        float randWait = Random.Range(0, 5);

        yield return new WaitForSeconds(randWait);

        clearFX.SetActive(true);

        StartCoroutine(Death(true));
    }
    #endregion
}
