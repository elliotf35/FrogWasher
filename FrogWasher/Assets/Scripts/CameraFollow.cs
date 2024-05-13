using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    void LateUpdate()
    {
        // Calculate new position with offset
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);


        // Smoothly interpolate camera's position
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
