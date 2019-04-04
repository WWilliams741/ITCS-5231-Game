using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    int agility = 5;
    Vector3 movementInput;
    Rigidbody rb;
    private bool blocking;
    [HideInInspector] public int damage = 2;
    [HideInInspector] public double exp = 0;
    [HideInInspector] public int level;
    [SerializeField] Animator anim;
    [SerializeField] private int vitality;
    private PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        data = SaveGameData.instance.playerData;
        //agility = data.agility;
        agility = 5;
        vitality = data.vitality;
        damage = data.strength;
        level = data.level;
        exp = data.exp;
        blocking = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimatePlayer();
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Die");
        }
    }

    void MovePlayer()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !anim.GetBool("Attacking"))
        {
            movementInput = new Vector3(Input.GetAxisRaw("Horizontal") * agility, 0f, Input.GetAxisRaw("Vertical") * agility);
            movementInput = transform.TransformDirection(movementInput);
            rb.velocity = movementInput;
        }
    }
    
    void AnimatePlayer()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Moving", true);
            }
            else
            {
                anim.SetBool("Moving", false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                anim.SetInteger("Attack", UnityEngine.Random.Range(0, 3));
                anim.SetBool("Attacking", true);
            }
            if(Input.GetMouseButtonUp(0))
            {
                anim.SetBool("Attacking", false);
            }
            if (Input.GetMouseButtonDown(1))
            {
                blocking = true;
                anim.SetBool("Blocking", blocking);
            }
            if(Input.GetMouseButtonUp(1))
            {
                blocking = false;
                anim.SetBool("Blocking", blocking);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Scene Transition")
        {
            //Set the players position
            //
        }
    }

    public void UpdateStats()
    {
        data.agility = agility;
        data.vitality = vitality;
        data.strength = damage;
        data.level = level;
        data.exp = exp;
        SaveGameData.instance.playerData = data;
    }

    public void DealDamage(int damage)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if(Physics.Raycast(ray, out hit, 1.5f))
        {
            Debug.Log("Enemy was hit by raycast");
            EnemyScript hitObject = hit.collider.gameObject.GetComponent<EnemyScript>();
            if(hitObject.tag.Equals("Enemy"))
            {
                hitObject.TakeDamage(damage);
                if(hitObject.sourceData.vitality <= 0) //If the enemy dies (health is less than or equals 0)
                {
                    exp += hitObject.sourceData.expDrop; //Add a certain amount to the experience (give experience function)
                    level = Log3(exp); //Call log2 to update the level and set to a temp level
                    //If the temp level is greater than the current level
                    //Show level up screen
                    Debug.Log("Exp is now " + exp);
                    Debug.Log("Level is now " + level);
                }

            }
        }
    }

    public void LevelUp()
    {
        //   `\(ツ)/`
    }

    //Leveling Utility 
    public int Log3(double experience)
    {
        return Mathf.FloorToInt((float)Math.Log(experience, 3));
    }
}
