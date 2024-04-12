using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform target;
    public float xOffset = 0.07f;
    private readonly float yOffset = -0.03f;
    private bool left;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xOffset = Math.Abs(xOffset);
        if (target.rotation.y == 1) {
            xOffset *= -1;
        }
        left = transform.parent.rotation.y != 0;
        // Debug.Log(left);
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the sprite to the mouse position
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );
        // Calculate the angle between the direction and the x-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (left) {
            angle *= -1;
            angle += 180;
        }
        // Debug.Log(angle);
        // angle = 0;
        
        if (left) {
            if (angle > 90 && angle < 180 ) {
                angle = 90;
            }
            if (angle >= 180 && angle < 270){
                angle = 270;
            }
        } else {
            if (angle < -90) {
                angle = -90;
            }
            if (angle > 90) {
                angle = 90;
            }
        }
        
        // Rotate the sprite to face the mouse position
        
        // transform.position = new Vector2(target.position.x + xOffset, target.position.y + yOffset);
        transform.localRotation= Quaternion.Euler(new Vector3(transform.parent.rotation.x, transform.parent.rotation.y, angle));
        // transform.rotation = new Quaternion(target.rotation.x, target.rotation.y, target.rotation.z, target.rotation.w);

    }
}
