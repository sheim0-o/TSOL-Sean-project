using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MobMovement : MonoBehaviour
{
    protected Path path;
    protected Animator animator;
    protected Rigidbody2D rigidbody2D;
    protected Vector3 spawnpoint;
    protected Transform target;
    protected Seeker seeker;
    protected bool isInChaseRange;
    protected int currentWaypoint = 1;
    protected bool reachedEndOfPath = false;

    [HideInInspector] public int randomSpot;
    [HideInInspector] public bool isWaiting = false;

    public Transform[] moveSpots;
    public float waitTime = 0.05F;

    public float speed = 200;

    public virtual void Walk(Rigidbody2D mRigidbody2D, Animator mAnimator) { }
    public virtual void UpdatePath(Seeker mSeeker, Vector3 pathTarget) { }
    public virtual void Patrul() 
    {
        switch(moveSpots.Length)
        {
            case 0:
                return;
            case 1:
                randomSpot = 0;
                return;
            default:
                if (randomSpot == null)
                    randomSpot = 0;
                if (!isWaiting && Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.5f)
                    StartCoroutine(coroutineTakeRandomSpot());
                return;
        }
    }
    public virtual IEnumerator coroutineTakeRandomSpot()
    {
        isWaiting = true;
        int newRS = randomSpot;
        while (newRS == randomSpot)
        {
            newRS = Random.Range(0, moveSpots.Length); ;
        }
        randomSpot = newRS;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
