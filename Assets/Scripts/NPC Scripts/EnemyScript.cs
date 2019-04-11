﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public NPCData sourceData;
    [SerializeField] private Animator anim;
    private NavMeshAgent agent;
    private bool patrolling;
    private Vector3 theDestination;
    public int health;
    private float radiusofSatisfaction;
    private float radiusofPatrol;
    private Vector3 startPoint;
    private Vector3 vision;
    private bool spotted;
    private GameObject theEnemy;

    private void Start()
    {
        spotted = false;
        patrolling = false;
        radiusofSatisfaction = 1f;
        radiusofPatrol = 20f;
        health = sourceData.vitality;
        agent = GetComponent<NavMeshAgent>();
        startPoint = transform.position;
        vision = transform.forward;
    }

    private void Update()
    {
        if (health > 0)
            anim.SetBool("Moving", true);
        vision = transform.forward;
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1.5f, transform.position.z), vision);
        //Plan on coding for enemy detection of player - different for boss (or just set radius of detection to be whole map lol
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.gameObject.tag.Equals("Player") && health > 0)
            {
                spotted = true;
                theEnemy = hit.collider.gameObject;
            }
        }

        Debug.DrawRay(new Vector3(transform.position.x, 1.5f, transform.position.z), vision * 10f, Color.red);

        if (spotted)
        {
            agent.SetDestination(theEnemy.transform.position);

            if (anim.name.Equals("Boss Controller"))
            {
                if (Vector3.Distance(transform.position, theEnemy.transform.position) <= 1.5f)
                {
                    agent.SetDestination(transform.position);
                    anim.SetBool("Attacking", true);
                    anim.SetInteger("Attack", Random.Range(0, 3));
                }
                else
                    anim.SetBool("Attacking", false);
            }
            else
            {
                if (Vector3.Distance(transform.position, theEnemy.transform.position) <= 1.5f)
                {
                    agent.SetDestination(transform.position);
                    anim.SetBool("Attacking", true);
                    anim.SetInteger("Attack", Random.Range(0, 2));
                }
                else
                    anim.SetBool("Attacking", false);
            }
        }

        if (!patrolling && !spotted && health > 0)
        {
            theDestination = Patrol();
            agent.SetDestination(theDestination);
        }
        else if (Vector3.Distance(theDestination, transform.position) <= radiusofSatisfaction)
        {
            patrolling = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            patrolling = false;
            spotted = false;
            anim.SetBool("Moving", false);
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }

        Debug.Log("Enemy Health is now: " + health);
    }

    public Vector3 Patrol()
    {
        Vector2 circlePoint = Random.insideUnitCircle * 5;
        Vector3 newPoint = new Vector3(circlePoint.x, gameObject.transform.position.y, circlePoint.y);

        while (Vector3.Distance(newPoint, startPoint) > radiusofPatrol)
        {
            circlePoint = Random.insideUnitCircle * 5;
            newPoint = new Vector3(circlePoint.x, gameObject.transform.position.y, circlePoint.y);

        }

        patrolling = true;
        return newPoint;
    }

    public void DealDamage()
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            Debug.Log("Enemy was hit by raycast");
            PlayerController hitObject = hit.collider.gameObject.GetComponent<PlayerController>();
            if (hitObject.tag.Equals("Player"))
            {
                hitObject.TakeDamage(sourceData.strength);
            }
        }
    }

    public void Attacking()
    {
        anim.SetBool("Attacking", false);
    }
}
