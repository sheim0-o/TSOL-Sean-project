using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chasing : MobMovement
{
    public override void Walk(Rigidbody2D mRigidbody2D, Animator mAnimator)
    {
        rigidbody2D = mRigidbody2D;
        animator = mAnimator;

        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", true);

        Vector3 dirVector = transform.GetComponent<MobBehaviour>().dirVector;

        if (transform.GetComponent<MobBehaviour>().isInChaseRange && !transform.GetComponent<MobBehaviour>().isInAttackRange)
        {
            Vector2 force = dirVector * speed * Time.deltaTime;
            rigidbody2D.AddForce(force);
            //rigidbody2D.MovePosition((Vector3)transform.position + (dirVector * speed * Time.deltaTime));
        }
    }
}
