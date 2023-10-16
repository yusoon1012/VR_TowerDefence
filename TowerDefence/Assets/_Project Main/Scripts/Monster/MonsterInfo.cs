using System;
using System.Collections;
using System.Collections.Generic;
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

    #region ���ϸ��̼�
    //! ������ ���� ���ϸ��̼� ���
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
                }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
            }       // if: ���ϸ��̼��� �� ������ ��� �н�

            float timeElpased = 0.0f;
            float duration = AniCheckLength();

            //! TODO: �Ÿ��� ����Ͽ� �Ÿ� ���� �ִ� ������Ʈ ü�� ��� �ϱ� �Ǵ� �ݶ��̴� ������ Trigger ���� ���� ü�� ��� �ϱ�

            attackFX.SetActive(true);

            while (timeElpased < duration)
            {
                timeElpased += Time.deltaTime;

                // TODO: ������ ȿ�� �������� �߰�

                yield return null;
            }
        }       // Animator ���
        else if (animation != null)
        {
            animation.Play("Attack");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Attack").length;

            attackFX.SetActive(true);

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                // TODO: ������ ȿ�� �������� �߰�

                yield return null;
            }
        }       // Animation ���

        this.gameObject.SetActive(false);
    }       // AttackPlayer()       // ����, ������, ȿ�� �߰� ����

    //! ������ �ɷ� ��� ���ϸ��̼� ���
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
                }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
            }       // if: ���ϸ��̼��� �� ������ ��� �н�

            // TODO: ����, ȿ��

            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Buffer"))
            {
                yield return null;
            }

            // TODO: �ɷ�
        }
        else if (animation != null)
        {
            animation.Play("Buffer");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Buffer").length;

            // TODO: ����, ȿ��

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // TODO: �ɷ�

            animation.Play("Walk");
        }

        isBuffer = false;
    }       // Buffer()     // ����, ȿ�� �߰� ����

    //! ������ �׾��� �� ���ϸ��̼� ���
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
                }       // loop: ���ϸ��̼��� JumpStart�� ���ö����� ���
            }       // if: ���ϸ��̼��� �� ������ ��� �н�

            float timeElpased = 0.0f;
            float duration = AniCheckLength();

            // TODO: ����, ȿ��

            while (timeElpased < duration)
            {
                timeElpased += Time.deltaTime;

                // TODO: ������ ȿ�� �������� �߰�

                yield return null;
            }
        }
        else if (animation != null)
        {
            animation.Play("Death");

            float timeElapsed = 0.0f;
            float duration = animation.GetClip("Death").length;

            // TODO: ����, ȿ��

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                // TODO: ������ ȿ�� �������� �߰�

                yield return null;
            }
        }

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
