using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator animator;
    private Transform transform;
    private Transform target;
    private Rigidbody2D rigidbody2D;
    private Seeker seeker;

    private MobBehaviour mobBehaviour;
    private MobAttacking mobAttacking;
    private MobMovement mobMovement;
    private MobDefeated mobDefeated;

    private Vector3 spawnpoint;
    public float checkRadius = 3;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        if (GameObject.FindWithTag("Player"))
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        mobBehaviour = GetComponent<MobBehaviour>();
        mobAttacking = GetComponent<MobAttacking>();
        mobMovement = GetComponent<MobMovement>();
        mobDefeated = GetComponent<MobDefeated>();

        spawnpoint = transform.position;
        mobBehaviour.health = mobBehaviour.maxHealth;
    }


    void Update()
    {
        mobBehaviour.target = target;
        mobBehaviour.Ranges(checkRadius, mobAttacking.attackRadius);
        mobBehaviour.Direction(target.position, animator);
     
        SetCondition();
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
            else if (mobBehaviour.isInChaseRange || (Vector3.Distance(transform.position, spawnpoint) > 0.5))
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
