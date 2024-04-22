using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PFChasing : MobMovement
{
    public float nextWaypointDistance = 0.4f;
    public override void UpdatePath(Seeker mSeeker, Vector3 pathTarget)
    {
        seeker = mSeeker;
        Vector3 enemyLoc = transform.GetComponent<BoxCollider2D>().bounds.center;
        seeker.StartPath(enemyLoc, pathTarget, OnPathComplete);

    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void Walk(Rigidbody2D mRigidbody2D, Animator mAnimator)
    {
        rigidbody2D = mRigidbody2D;
        animator = mAnimator;
        Vector3 enemyLoc = transform.GetComponent<BoxCollider2D>().bounds.center;

        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", true);

        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;

        Vector3 direction = (path.vectorPath[currentWaypoint] - enemyLoc).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rigidbody2D.AddForce(force);

        float distance = Vector2.Distance(enemyLoc, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }
}