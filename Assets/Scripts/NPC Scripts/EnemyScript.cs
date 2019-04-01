using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    [SerializeField] private Animator anim;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }
        
        Debug.Log("Health is now: " + health);
    }
}
