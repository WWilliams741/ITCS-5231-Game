using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool touchingEnemy;
    float speed;
    Vector3 movementInput;
    Rigidbody rb;
    private EnemyScript enemyTouched;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        touchingEnemy = false;
        speed = 7f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimatePlayer();
        Debug.DrawLine(transform.position, Vector3.forward, Color.red);
    }

    void MovePlayer()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !anim.GetBool("Attacking"))
        {
            movementInput = new Vector3(Input.GetAxisRaw("Horizontal") * speed, 0f, Input.GetAxisRaw("Vertical") * speed);
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetInteger("Attack", Random.Range(0, 3));
                anim.SetBool("Attacking", true);
            }
            else
            {
                anim.SetBool("Attacking", false);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            touchingEnemy = true;
            enemyTouched = other.gameObject.GetComponent<EnemyScript>();
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            touchingEnemy = false;
            enemyTouched = null;
        }
    }

    public void DealDamage(int damage)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            Debug.Log("Enemy was hit by raycast");
            GameObject hitObject = hit.collider.gameObject;
            if(hitObject.tag.Equals("Enemy") && Vector3.Distance(transform.position, hitObject.transform.position) < 3)
            {
                hitObject.GetComponent<EnemyScript>().TakeDamage(damage);
            }
        }
    }
}
