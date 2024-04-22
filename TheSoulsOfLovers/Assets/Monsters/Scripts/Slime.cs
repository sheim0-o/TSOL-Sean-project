using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Animator animator;
    private Transform transform;
    private Transform target;
    private Rigidbody2D rigidbody2D;
    private Seeker seeker;
    private Vector3 spawnpoint;
    private Vector3 pathTarget;

    private MobBehaviour mobBehaviour;
    private MobAttacking mobAttacking;
    private MobMovement mobMovement;
    private MobDefeated mobDefeated;

    public Transform zoneOfExistence;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        if (GameObject.FindWithTag("Player"))
        {
            target = GameObject.FindWithTag("Player").transform;
            pathTarget = target.position;
        }
        
        mobBehaviour = GetComponent<MobBehaviour>();
        mobAttacking = GetComponent<MobAttacking>();
        mobMovement = GetComponent<MobMovement>();
        mobDefeated = GetComponent<MobDefeated>();


        InvokeRepeating("UpdatePath", 0f, 0.5f);

        spawnpoint = transform.position;
        mobBehaviour.health = mobBehaviour.maxHealth;
    }

    void Update()
    {
        mobBehaviour.target = target;
        mobBehaviour.Ranges(zoneOfExistence, mobAttacking.attackRadius);
        mobBehaviour.Direction(pathTarget, animator);
        
        SetCondition();
    }

    void UpdatePath()
    {
        Vector3 playerLoc = target.GetComponent<BoxCollider2D>().bounds.center;
        if (mobBehaviour.isInChaseRange)
        {
            pathTarget = playerLoc;
            mobMovement.UpdatePath(seeker, playerLoc);
        }
        else
        {
            mobMovement.Patrul();
            if (mobMovement.moveSpots.Length != 0)
            {
                pathTarget = mobMovement.moveSpots[mobMovement.randomSpot].position;
                mobMovement.UpdatePath(seeker, mobMovement.moveSpots[mobMovement.randomSpot].position);
            }
            else
            {
                pathTarget = spawnpoint;
                mobMovement.UpdatePath(seeker, spawnpoint);
            }
        }
    }

    public void SetCondition()
    {
        if (mobBehaviour.health <= 0)
        {
            mobDefeated.Destroyed(transform, rigidbody2D, animator);
        }
        else
        {
            if (mobBehaviour.isInAttackRange)
            {
                if (mobAttacking.canAttack)
                    mobAttacking.Attack(animator, transform, target);
                else
                    mobBehaviour.Idle(animator, rigidbody2D);
            }
            else if (mobBehaviour.isInChaseRange || (Vector3.Distance(transform.position, pathTarget) > 0.5F))
            {
                mobMovement.Walk(rigidbody2D, animator);
            }
            else
                mobBehaviour.Idle(animator, rigidbody2D);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("HitBoxOfPlayerWeapon") && mobBehaviour.health > 0)
        {
            mobBehaviour.health--;
            animator.Play("Hurt");
            Debug.Log("Enemy health = " + mobBehaviour.health);
        }
    }
}
