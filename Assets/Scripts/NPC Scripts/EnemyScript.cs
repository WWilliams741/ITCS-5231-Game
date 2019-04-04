using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public NPCData sourceData;
    [SerializeField] private Animator anim;
    public int health;

    private void Start()
    {
        health = sourceData.vitality;
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
}
