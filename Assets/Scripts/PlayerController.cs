using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed;
    Vector3 movementInput;
    Rigidbody rb;
    private bool blocking;
    [SerializeField] Animator anim;
    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7f;
        health = 100;
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
        Debug.DrawRay(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 2f, Color.red);
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

            if (Input.GetMouseButtonDown(0))
            {
                anim.SetInteger("Attack", Random.Range(0, 3));
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

    public void DealDamage(int damage)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1.5f);
        if(Physics.Raycast(ray, out hit, 1.5f))
        {
            Debug.Log("Enemy was hit by raycast");
            GameObject hitObject = hit.collider.gameObject;
            if(hitObject.tag.Equals("Enemy"))
            {
                hitObject.GetComponent<EnemyScript>().TakeDamage(damage);
            }
        }
    }
}
