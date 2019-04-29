using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Vector3 movementInput;
    [SerializeField] Rigidbody rb;
    [SerializeField] public Animator anim;
    [SerializeField] Canvas levelUpMenu;
    [SerializeField] TMP_Dropdown levelUpOptions;
    [SerializeField] TextMeshProUGUI agilityText;
    [SerializeField] TextMeshProUGUI strengthText;
    [SerializeField] TextMeshProUGUI vitalityText;
    [SerializeField] Image healthBar;
    [SerializeField] Image healthBackground;
    [SerializeField] MusicManager sounds;

    private bool blocking;

    private PlayerData data;

    private int vitality;
    private int maxVitality;
    private int strength;
    private int agility;
    private double exp;
    private float vitalityPercent;
    private int level;
    private bool alive;

    private bool canSave;
    public bool bossDead;

    public static PlayerController instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {

        data = SaveGameData.instance.playerData;
        alive = true;
        blocking = false;
        vitalityPercent = 1f;
        agilityText.text = "";
        strengthText.text = "";
        vitalityText.text = "";
        sounds.SetMountainMusic();
        if (data.agility != 0) {
            StartCoroutine(loadCorrectScene());
            //agility = data.agility;
            agility = data.agility;
            vitality = data.vitality;
            maxVitality = data.vitality;
            strength = data.strength;
            level = data.level;
            exp = data.exp;
            transform.position = new Vector3 (data.position[0], data.position[1], data.position[2]);
        } else {
            agility = 5;
            vitality = 100;
            maxVitality = 100;
            strength = 2;
            level = 1;
            exp = 0;
        }

        canSave = false;
        bossDead = false;
    }

    // Update is called once per frame
    void Update() {
        HealthBar();
        MovePlayer();
        AnimatePlayer();
        agilityText.text = "Agility (Max 10): " + agility;
        strengthText.text = "Strength: " + strength;
        vitalityText.text = "Vitality: " + maxVitality;

        if (vitality <= 0 && alive) {
            alive = false;
            anim.SetTrigger("Die");
        }

        if (canSave && Input.GetKeyDown(KeyCode.E)) {
            print("saving");
            vitality = maxVitality;
            UpdateStats();
        }
        if(SceneManager.GetActiveScene().name == "Credits")
        {
            Destroy(gameObject);
        }
    }

    void MovePlayer() {
        if (CanMove()) {
            movementInput = new Vector3(Input.GetAxisRaw("Horizontal") * agility, rb.velocity.y, Input.GetAxisRaw("Vertical") * agility);
            movementInput = transform.TransformDirection(movementInput);
            rb.velocity = movementInput;
        } else {
            movementInput = new Vector3(0.0f, 0.0f, 0.0f);
            movementInput = transform.TransformDirection(movementInput);
            rb.velocity = movementInput;
        }
    }

    void AnimatePlayer() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
                anim.SetBool("Moving", true);
            } else {
                anim.SetBool("Moving", false);
            }

            if (Input.GetMouseButtonDown(0) && !(anim.GetBool("Attacking") || anim.GetBool("Blocking"))) {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
                    anim.SetInteger("Attack", UnityEngine.Random.Range(0, 3));
                    anim.SetBool("Attacking", true);
                }

            }
            if (Input.GetMouseButtonDown(1) && !anim.GetBool("Attacking")) {
                blocking = true;
                anim.SetBool("Blocking", blocking);
            }
            if (Input.GetMouseButtonUp(1)) {
                blocking = false;
                anim.SetBool("Blocking", blocking);
            }

        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Gateway" && bossDead) {
            sounds.SetForestMusic();
            SaveGameData.instance.SaveData();
            SceneManager.LoadScene("Forest Scene");
        }

        if (other.gameObject.tag == "Boss Gateway" && bossDead) {
            print("boss is dead");
            sounds.SetFinalBossMusic();
            SaveGameData.instance.SaveData();
            SceneManager.LoadScene("Boss Scene");
        }

        if (other.gameObject.tag == "Boss Area") {
            sounds.SetMiniBossMusic();
        }

        if (other.gameObject.tag == "Final Boss Area") {
            sounds.SetFinalBossMusic();
        }

        if (other.gameObject.tag == "Save Point") {
            print("setting save to true");
            canSave = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Save Point") {
            print("setting save to false");
            canSave = false;
        }
    }

    public void UpdateStats() {
        data.agility = agility;
        data.vitality = vitality;
        data.strength = strength;
        data.level = level;
        data.exp = exp;
        data.position[0] = transform.position.x;
        data.position[1] = transform.position.y;
        data.position[2] = transform.position.z;
        data.scene = SceneManager.GetActiveScene().name;
        print(SceneManager.GetActiveScene().name);
        SaveGameData.instance.playerData = data;
        SaveGameData.instance.SaveData();
    }

    public void DealDamage() {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if (Physics.Raycast(ray, out hit, 1.5f)) {
            Debug.Log("Enemy was hit by raycast");
            EnemyScript hitObject = hit.collider.gameObject.GetComponent<EnemyScript>();
            if (hitObject.tag.Equals("Enemy") || hitObject.tag.Equals("Final Boss")) {
                hitObject.TakeDamage(strength);
                if (hitObject.health <= 0 && !hitObject.tag.Equals("Final Boss")) //If the enemy dies (health is less than or equals 0)
                {
                    exp += hitObject.sourceData.expDrop; //Add a certain amount to the experience (give experience function)
                    int tempLevel = Log3(exp);
                    if (tempLevel > level) {
                        levelUpMenu.gameObject.SetActive(true);
                        Debug.Log("Menu should be shown now");
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
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

    public void LevelUp() {
        Cursor.lockState = CursorLockMode.Locked;
        int stat = levelUpOptions.value;
        switch (stat) {
            case 0: // Vitality
                print("vitality updated");
                maxVitality += 10;
                healthBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVitality, 25);
                healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVitality, 25);
                break;
            case 1: // Strength
                print("strength updated");
                strength++;
                break;
            case 2: // Agility
                if (agility == 10) {
                    print("agility at max");
                    return;
                }
                print("agility updated");
                agility++;
                break;
        }
        vitality = maxVitality;
        level++;
        UpdateStats();
        // set the levelUp menu to inactive;
        levelUpMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        HealthBar();
    }

    //Leveling Utility 
    public int Log3(double experience) {
        return Mathf.FloorToInt((float)Math.Log(experience, 3));
    }

    public void Attacking() {
        anim.SetBool("Attacking", false);
    }

    private bool CanMove() {
        return Cursor.lockState == CursorLockMode.Locked && !(anim.GetBool("Attacking") || blocking) && alive;
    }

    public void TakeDamage(int damage) {
        if (blocking && alive) {
            damage = 0;
        }
        vitality -= damage;
        if (vitality <= 0 && alive) {
            alive = false;
            anim.SetTrigger("Die");
        }
    }

    public int GetVitality() {
        return vitality;
    }

    public bool BossDead() {
        return bossDead;
    }

    public void setBossDead(bool _bossDead) {
        bossDead = _bossDead;
    }

    private void HealthBar()
    {
        vitalityPercent = (float)vitality / maxVitality;
        healthBar.fillAmount = vitalityPercent;
        Debug.Log(healthBar.GetComponent<RectTransform>().sizeDelta.x);
        Debug.Log(healthBackground.GetComponent<RectTransform>().sizeDelta.x);
        Debug.Log(vitalityPercent);
    }

   IEnumerator loadCorrectScene() {
        SceneManager.LoadScene(data.scene);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(data.scene));
    }
}
