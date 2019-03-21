using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed;
    Vector3 movementInput;
    Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7f;
        health = 100;
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
        Debug.DrawRay(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1f, Color.red);
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

    public void DealDamage(int damage)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1f, transform.position.z), transform.forward * 1f);
        if(Physics.Raycast(ray, out hit, 1f))
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
