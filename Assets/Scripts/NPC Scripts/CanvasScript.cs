using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private Camera enemyHealthBar;

    // Update is called once per frame
    void Update()
    {
        Vector3 thePosition = enemyHealthBar.transform.position - transform.position;
		
		thePosition.x = thePosition.z = 0f;
		transform.LookAt(enemyHealthBar.transform.position - thePosition);
		transform.Rotate(0,180,0);
    }
}
