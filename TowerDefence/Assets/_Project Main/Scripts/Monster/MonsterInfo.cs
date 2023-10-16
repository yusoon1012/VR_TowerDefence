using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 * 2023 - 10 - 15
 * ���� �ִϸ��̼ǿ� �°� ȿ�� ����
 * �۸��� �ʱ� �ִϸ��̼� �ȱ� ����
 * �۸��� ���� �� �� ȿ�� ����
 * ���� ���� ����
 * ���� ã��
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
        // TODO: �÷��̾� ü�� 0�ʰ����� üũ�ϴ� if�� �ۼ� -------------------------------------------
        // �÷��̾�� ���Ͱ��� �Ÿ� ���
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

    //! ���Ͱ� �÷��̾ ã�ư��� �׺���̼�
    private void TargetPlayer()
    {
        if (distance < this.recognitionRange)
        {
            nav.SetDestination(player.transform.position);
        }
    }       // TargetPlayer()

    //! ������ ȿ�� �ڷ�ƾ
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

    //! SkinnedMeshRenderer ������Ʈ�� ã�� ����Լ�
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

    #region ���ϸ��̼�
    //! ������ ���� ���ϸ��̼� ���
    private IEnumerator Attack()
    {
        isAttack = true;

        animator.SetTrigger("Attack");

        if (!AniCheckInfo("Attack"))
        {
            while (!AniCheckInfo("Attack"))
            {
                yield return null;
            }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
        }       // if: ���ϸ��̼��� �� ������ ��� �н�

        float duration = AniCheckLength();

        //! TODO: �Ÿ��� ����Ͽ� �Ÿ� ���� �ִ� ������Ʈ ü�� ��� �ϱ� �Ǵ� �ݶ��̴� ������ Trigger ���� ���� ü�� ��� �ϱ�

        attackFX.SetActive(true);

        StartCoroutine(SetDissolve(duration, splitEndValue, splitStartValue));

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }       // AttackPlayer()       // ����, ������, ȿ�� �߰� ����

    //! ������ �ɷ� ��� ���ϸ��̼� ���
    private IEnumerator Buffer()
    {
        isBuffer = true;

        animator.SetTrigger("Buffer");

        if (!AniCheckInfo("Buffer"))
        {
            while (!AniCheckInfo("Buffer"))
            {
                yield return null;
            }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
        }       // if: ���ϸ��̼��� �� ������ ��� �н�

        // TODO: ����, ȿ��

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Buffer"))
        {
            yield return null;
        }

        // TODO: �ɷ�

        isBuffer = false;
    }       // Buffer()     // ����, ȿ�� �߰� ����

    //! ������ �׾��� �� ���ϸ��̼� ���
    private IEnumerator Death()
    {
        isDeath = true;

        animator.SetTrigger("Death");

        if (!AniCheckInfo("Death"))
        {
            while (!AniCheckInfo("Death"))
            {
                yield return null;
            }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
        }       // if: ���ϸ��̼��� �� ������ ��� �н�

        float duration = AniCheckLength();

        // TODO: ����, ȿ��

        StartCoroutine(SetDissolve(duration, splitEndValue, splitStartValue));

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }       // Death()      // ����, ������, ȿ�� �߰� ����
    #endregion

    #region �ʱ�ȭ
    //! ������Ʈ Ǯ���� ���� ����
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

    #region ���ϸ��̼� ����
    //! ���� ����ǰ� �ִ� ���ϸ��̼� üũ
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

    //! ���� ����ǰ� �ִ� ���ϸ��̼� Ŭ�� ����
    private float AniCheckLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    #endregion

    #region ���� �Ӽ��� �����ֱ�
    //! ������ ���� ���� �ֱ�
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
