using System.Collections;
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

    private void Start()
    {
        patrolling = false;
        radiusofSatisfaction = 1f;
        health = sourceData.vitality;
        agent = GetComponent <NavMeshAgent> ();
    }

    private void Update()
    {

        //Plan on coding for enemy detection of player - different for boss (or just set radius of detection to be whole map lol


        if (!patrolling)
        {
            patrolling = true;
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
        if(health <= 0)
        {
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }
        
        Debug.Log("Enemy Health is now: " + health);
    }

    public Vector3 Patrol()
    {
        Vector2 circlePoint = Random.insideUnitCircle * 5;
        Vector3 newPoint = new Vector3(circlePoint.x, gameObject.transform.position.y, circlePoint.y);

        return newPoint;
    }
}
