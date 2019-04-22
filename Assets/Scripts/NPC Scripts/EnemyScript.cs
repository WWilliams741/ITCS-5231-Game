using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public NPCData sourceData;
    [SerializeField] private Animator anim;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private PlayerController Player;
    private float healthPercent;
    private float maxHealth;
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
        radiusofPatrol = 10f;
        health = sourceData.vitality;
        agent = GetComponent<NavMeshAgent>();
        startPoint = transform.position;
        vision = transform.forward;
        anim.SetBool("Moving", true);
        healthPercent = 1f;
        maxHealth = sourceData.vitality;
    }

    private void Update()
    {
        vision = transform.forward;
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1.5f, transform.position.z), vision);
        //Plan on coding for enemy detection of player - different for boss (or just set radius of detection to be whole map lol

        healthPercent = health / maxHealth;
        healthBarImage.fillAmount = healthPercent;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.gameObject.tag.Equals("Player") && health > 0)
            {
                spotted = true;
                theEnemy = hit.collider.gameObject;
            }
        }

        if (spotted && health > 0)
        {
            if (Vector3.Distance(transform.position, theEnemy.transform.position) > 1.5f)
            {
                agent.SetDestination(theEnemy.transform.position);
                anim.SetBool("Moving", true);
            }
            else
            {
                agent.SetDestination(transform.position);
                anim.SetBool("Moving", false);
            }

            if (anim.GetLayerName(0).Equals("Boss"))
            {
                //print("we are a boss bitches");
                //anim.SetBool("Attacking", true);
                //anim.SetInteger("Attack", Random.Range(0, 3));
                //StartCoroutine(resetAttack());
                if (Vector3.Distance(transform.position, theEnemy.transform.position) <= 1.5f && !anim.GetBool("Attacking"))
                {
                    anim.SetBool("Attacking", true);
                    anim.SetInteger("Attack", Random.Range(0, 3));
                    StartCoroutine(resetAttack());
                }
                //else if (!anim.GetBool("Attacking"))
                //{
                //    anim.SetBool("Moving", true);
                //}
                //else if (health > 0 && !anim.GetBool("Attacking"))
                //{
                //    anim.SetBool("Attacking", false);
                //    anim.SetBool("Moving", true);
                //}

            }
            else
            {
                if (Vector3.Distance(transform.position, theEnemy.transform.position) <= 1.5f && !anim.GetBool("Attacking"))
                {
                    anim.SetBool("Attacking", true);
                    anim.SetInteger("Attack", Random.Range(0, 2));
                    StartCoroutine(resetAttack());
                }
                //else if (!anim.GetBool("Attacking"))
                //{
                //    anim.SetBool("Moving", true);
                //}

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
            enemyCanvas.gameObject.SetActive(false);
        }
    }

    public Vector3 Patrol()
    {
        Vector2 circlePoint = Random.insideUnitCircle * radiusofPatrol;
        Vector3 newPoint = new Vector3(startPoint.x, transform.position.y, startPoint.z);
        newPoint.x += circlePoint.x;
        newPoint.z += circlePoint.y;

        patrolling = true;
        return newPoint;
    }

    public void DealDamage()
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            PlayerController hitObject = hit.collider.gameObject.GetComponent<PlayerController>();
            if (hitObject.tag.Equals("Player"))
            {
                hitObject.TakeDamage(sourceData.strength);
            }
        }
    }

    public IEnumerator resetAttack()
    {
        yield return null;

        anim.SetBool("Attacking", false);
    }

    public void BossIsDead(int value) {
        if (value == 1) {
            Player.setBossDead(true);
        }
    }
}
