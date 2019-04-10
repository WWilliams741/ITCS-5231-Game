using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Vector3 movementInput;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] Canvas levelUpMenu;
    [SerializeField] TMP_Dropdown levelUpOptions;
    [SerializeField] TextMeshProUGUI agilityText;
    [SerializeField] TextMeshProUGUI strengthText;
    [SerializeField] TextMeshProUGUI vitalityText;

    private bool blocking;

    private PlayerData data;

    private int vitality;
    private int strength;
    private int agility;
    private double exp;
    private int level;
    private bool alive;

    // Start is called before the first frame update
    void Start()
    {
        data = SaveGameData.instance.playerData;
        alive = true;
        blocking = false;
        agilityText.text = "";
        strengthText.text = "";
        vitalityText.text = "";
        if (data.agility != 0)
        {
            //agility = data.agility;
            agility = data.agility;
            vitality = data.vitality;
            strength = data.strength;
            level = data.level;
            exp = data.exp;
        }
        else
        {
            agility = 5;
            vitality = 50;
            strength = 2;
            level = 1;
            exp = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(anim.GetBool("Attacking"));
        MovePlayer();
        AnimatePlayer();
        agilityText.text = "Agility (Max 10): " + agility;
        strengthText.text = "Strength: " + strength;
        vitalityText.text = "Vitality: " + vitality;
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Die");
        }
    }

    void MovePlayer()
    {
        if (CanMove())
        {
            movementInput = new Vector3(Input.GetAxisRaw("Horizontal") * agility, rb.velocity.y, Input.GetAxisRaw("Vertical") * agility);
            movementInput = transform.TransformDirection(movementInput);
            rb.velocity = movementInput;
        }
		else {
			movementInput = new Vector3(0.0f,0.0f,0.0f);
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

            if (Input.GetMouseButtonDown(0) && !anim.GetBool("Attacking"))
            {
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    anim.SetInteger("Attack", UnityEngine.Random.Range(0, 3));
                    anim.SetBool("Attacking", true);
                }
                
            }

            if (Input.GetMouseButtonDown(1) && !anim.GetBool("Attacking"))
            {
                blocking = true;
                anim.SetBool("Blocking", blocking);
            }
            if (Input.GetMouseButtonUp(1))
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
        data.strength = strength;
        data.level = level;
        data.exp = exp;
        SaveGameData.instance.playerData = data;
        SaveGameData.instance.SaveData();
    }

    public void DealDamage(int strength)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if(Physics.Raycast(ray, out hit, 1.5f))
        {
            Debug.Log("Enemy was hit by raycast");
            EnemyScript hitObject = hit.collider.gameObject.GetComponent<EnemyScript>();
            if(hitObject.tag.Equals("Enemy"))
            {
                hitObject.TakeDamage(strength);
                if(hitObject.health <= 0) //If the enemy dies (health is less than or equals 0)
                {
                    exp += hitObject.sourceData.expDrop; //Add a certain amount to the experience (give experience function)
                    int tempLevel = Log3(exp);
                    if (tempLevel > level)
                    {
                        levelUpMenu.gameObject.SetActive(true);
                        Debug.Log("Menu should be shown now");
                        Cursor.lockState = CursorLockMode.None;
                    }
                    //Call log3 to update the level and set to a temp level
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
        Cursor.lockState = CursorLockMode.Locked;
        int stat = levelUpOptions.value;
        switch (stat)
        {
            case 0: // Vitality
                print("vitality updated");
                vitality++;
                break;
            case 1: // Strength
                print("strength updated");
                strength++;
                break;
            case 2: // Agility
                if (agility == 10)
                {
                    print("agility at max");
                    return;
                }
                print("agility updated");
                agility++;
                break;
        }
        level++;
        UpdateStats();
        // set the levelUp menu to inactive;
        levelUpMenu.gameObject.SetActive(false);
    }

    //Leveling Utility 
    public int Log3(double experience)
    {
        return Mathf.FloorToInt((float)Math.Log(experience, 3));
    }

    public void Attacking()
    {
        print("setting attack back to false");
        anim.SetBool("Attacking", false);
    }

    private bool CanMove()
    {
        return Cursor.lockState == CursorLockMode.Locked && !(anim.GetBool("Attacking") || blocking) && alive;
    }


    /*
     * 
     * When leveling up /gaining experience enable levelUp menu and check for agility
     *     
     * 
     */
}
