using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MobAttacking : MonoBehaviour
{
    protected Animator animator;
    protected Coroutine coroutine;
    protected Transform transform;
    protected Transform target;

    public bool canAttack = true;

    public float attackCooldown = 3;
    public float damage = 1;
    public float attackRadius = 1;
    public virtual void Attack(Animator mAnimator, Transform mTransform, Transform mTarget)
    {
        animator = mAnimator;
        transform = mTransform;
        target = mTarget;

        animator.PlayInFixedTime("Attack");
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Coroutine("HitEnemy", animatorStateInfo.length/2);
        Coroutine("Attack", attackCooldown);
    }
    public virtual void Coroutine(string type, float cooldownTime)
    {
        switch (type)
        {
            case "Attack":
                coroutine = StartCoroutine(coroutineAttack(cooldownTime));
                break;
            case "HitEnemy":
                coroutine = StartCoroutine(coroutineHitEnemy(cooldownTime));
                break;
        }
    }

    public virtual IEnumerator coroutineHitEnemy(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);

        Vector3 enemyLoc = transform.GetComponent<BoxCollider2D>().bounds.center;
        Vector3 playerLoc = target.GetComponent<BoxCollider2D>().bounds.center;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (target.GetComponent<Player>().health > 0 && (Vector3.Distance(enemyLoc, playerLoc) < attackRadius))
                target.GetComponent<Player>().Hit(damage);

        }
    }
    public virtual IEnumerator coroutineAttack(float cooldownTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }
}
