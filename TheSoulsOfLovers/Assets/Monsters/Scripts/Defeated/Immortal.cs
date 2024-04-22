using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immortal : MobDefeated
{
    public float destroyedCooldown = 3;
    protected Coroutine coroutine;
    public override void Destroyed(Transform mTransform, Rigidbody2D mRigidbody2D, Animator mAnimator)
    {
        transform = mTransform;
        rigidbody2D = mRigidbody2D;
        animator = mAnimator;

        if (!wait)
            Coroutine("DestroyedStart", destroyedCooldown);
    }
    public void Coroutine(string type, float cooldownTime)
    {
        switch (type)
        {
            case "DestroyedStart":
                coroutine = StartCoroutine(coroutineDestroyedStart(cooldownTime));
                break;

            case "DestroyedEnd":
                coroutine = StartCoroutine(coroutineDestroyedEnd(cooldownTime));
                break;
        }
    }
    public IEnumerator coroutineDestroyedStart(float cooldownTime)
    {
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.isKinematic = true;
        wait = true;
        animator.Play("DestroyedStart");
        yield return new WaitForSeconds(cooldownTime);

        animator.Play("DestroyedEnd");
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Coroutine("DestroyedEnd", animatorStateInfo.length);
    }
    public IEnumerator coroutineDestroyedEnd(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        transform.GetComponent<MobBehaviour>().health = transform.GetComponent<MobBehaviour>().maxHealth;
        rigidbody2D.isKinematic = false;
        wait = false;
        animator.Play("Idle");
    }
}
