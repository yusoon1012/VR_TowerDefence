using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();

        Death();
    }

    public void Jump()
    {
        ani.SetTrigger("Jump");
    }

    public void Buffer()
    {
        ani.SetTrigger("Buffer");
    }

    public void Attack()
    {
        ani.SetTrigger("Attack");
    }

    public void Death()
    {
        ani.SetTrigger("Death");
    }
}
