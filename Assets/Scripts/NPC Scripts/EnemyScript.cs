using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public NPCData sourceData;
    [SerializeField] private Animator anim;

    public void TakeDamage(int damage)
    {
        sourceData.vitality -= damage;
        if(sourceData.vitality <= 0)
        {
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }
        
        Debug.Log("sourceData.vitality is now: " + sourceData.vitality);
    }
}
