using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBehaviour : MonoBehaviour
{
    public float maxHealth = 3;
    public bool shouldRotate = true;
    public float hurtTime = 1.5F;

    [HideInInspector]
    public bool isInChaseRange, isInAttackRange, isHurt;
    [HideInInspector]
    public Rigidbody2D rigidbody2D;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public float health;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Vector3 dirVector;

    private GridGraph gg;

    private void Start()
    {
        /*
        PlayerPrefs playerPrefs = GameObject.FindWithTag("PlayerPrefs").GetComponent<PlayerPrefs>();
        GameData.DefeatedEnemy enemy = playerPrefs.locations[playerPrefs.location].DefeatedEnemies.Find(x =>
            (x.location == playerPrefs.location) && (x.scene == playerPrefs.scene) && (x.name == transform.name));
        if (enemy != null)
            gameObject.SetActive(false);
        */
    }

    public virtual void Idle(Animator animator, Rigidbody2D rigidbody2D)
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", false);
        rigidbody2D.velocity = Vector3.zero;
    }
    public void Direction(Vector3 targetPos, Animator animator)
    {
        dirVector = targetPos - transform.position;
        dirVector.Normalize();
        animator.SetFloat("MoveX", dirVector.x);
        animator.SetFloat("MoveY", dirVector.y);
    }
    public void Hurt() { Coroutine coroutine = StartCoroutine(coroutineHurt()); }
    public virtual IEnumerator coroutineHurt()
    {
        health--;
        Debug.Log("Enemy health = " + health);

        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", false);
        animator.SetBool("isHurting", true);

        isHurt = true;
        animator.Play("Hurt");
        yield return new WaitForSeconds(hurtTime);
        animator.SetBool("isHurting", false);
        isHurt = false;
    }

    public void Ranges(Transform zoneOfExistence, float attackRadius = 0)
    {
        if(gg==null)
        {
            AstarPath graphCollision = zoneOfExistence.GetComponent<AstarPath>();
            gg = (GridGraph)graphCollision.graphs[0];
            zoneOfExistence.GetComponent<BoxCollider2D>().offset.Set(gg.center.x, gg.center.y);
            zoneOfExistence.GetComponent<BoxCollider2D>().size.Set(gg.width*gg.nodeSize, gg.depth * gg.nodeSize);
        }
        Vector3 enemyLoc = transform.GetComponent<BoxCollider2D>().bounds.center;
        Vector3 playerLoc = target.GetComponent<BoxCollider2D>().bounds.center;
        Bounds zoneBounds = zoneOfExistence.GetComponent<BoxCollider2D>().bounds;


        if (Vector3.Distance(enemyLoc, playerLoc) < attackRadius)
            isInAttackRange = true;
        else isInAttackRange = false;

        if(zoneBounds.Contains(playerLoc))
            isInChaseRange = true;
        else isInChaseRange = false;
    }
    public void Ranges(float checkRadius, float attackRadius = 0)
    {
        Vector3 enemyLoc = transform.GetComponent<BoxCollider2D>().bounds.center;
        Vector3 playerLoc = target.GetComponent<BoxCollider2D>().bounds.center;

        if (Vector3.Distance(enemyLoc, playerLoc) < attackRadius)
            isInAttackRange = true;
        else isInAttackRange = false;

        if (Vector3.Distance(transform.position, target.position) < checkRadius) isInChaseRange = true; else isInChaseRange = false;
    }
}
