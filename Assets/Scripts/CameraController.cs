using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
     Vector2 lookDirection;
    Vector2 smoothingVector;
    GameObject player;

    [SerializeField]
    float sensitivity;

    [SerializeField]
    float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CameraMovement()
    {
        Vector2 mouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDirection = Vector2.Scale(mouseDirection, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothingVector.x = Mathf.Lerp(smoothingVector.x, mouseDirection.x, 1f / smoothing);
        smoothingVector.y = Mathf.Lerp(smoothingVector.y, mouseDirection.y, 1f / smoothing);
        lookDirection += smoothingVector;
        lookDirection.y = Mathf.Clamp(lookDirection.y, -50f, 0); //Limit the vertical mouse movement
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            transform.localRotation = Quaternion.AngleAxis(-lookDirection.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(lookDirection.x, player.transform.up);
        }
    }
}
