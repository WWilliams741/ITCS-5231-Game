using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed;
    Vector3 movementInput;
    Rigidbody rb;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimatePlayer();
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
        print("Dealing: " + damage + " damage.");
    }
}
