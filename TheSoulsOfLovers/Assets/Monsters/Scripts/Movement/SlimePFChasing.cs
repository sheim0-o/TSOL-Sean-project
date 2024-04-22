using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlimePFChasing : PFChasing
{
    Coroutine coroutine;
    bool canMove = true;
    bool canJump = true;
    Vector2 force;

    public float cooldownTimeOfJump = 3;

    public override void Walk(Rigidbody2D mRigidbody2D, Animator mAnimator)
    {
        rigidbody2D = mRigidbody2D;
        animator = mAnimator;

        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2D.position);
        force = direction * speed * Time.deltaTime;

        if (canMove)
        {
            rigidbody2D.AddForce(force);
            if (canJump)
            {
                animator.Play("Jump");
                AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                Coroutine("Jump", animatorStateInfo.length);
            }
        }

        float distance = Vector2.Distance(rigidbody2D.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }
    public virtual void Coroutine(string type, float cooldownTime)
    {
        switch (type)
        {
            case "Jump":
                coroutine = StartCoroutine(coroutineJump(cooldownTime));
                break;
            case "Wait":
                coroutine = StartCoroutine(coroutineWait(cooldownTime));
                break;
        }
    }

    public virtual IEnumerator coroutineJump(float cooldownTime)
    {
        canJump = false;
        yield return new WaitForSeconds(cooldownTime);
        canMove = false;
        Coroutine("Wait", cooldownTimeOfJump);
    }
    public virtual IEnumerator coroutineWait(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canJump = true;
        canMove = true;
    }
}
