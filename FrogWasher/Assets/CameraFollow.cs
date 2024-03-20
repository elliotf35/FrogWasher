using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2.5f;
    public Transform target;
    public float yPos = -4.27f;
    // Update is called once per frame
    void Start()
    {
        transform.position = new Vector3(target.position.x,  yPos, -10f);
    }
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, yPos, -10f);
        transform.position = Vector3.Slerp(transform.position,newPos,FollowSpeed*Time.deltaTime);
    }
}