using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float maxSpeed;
    public int startingPoint;
    public Transform[] points;
    private int i;
    private float minSpeed = 0.1f; // Minimum speed to avoid the platform stopping completely before it reaches the point

    void Start()
    {
        transform.position = points[startingPoint].position;
        i = startingPoint;  // Ensure 'i' starts at 'startingPoint'
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, points[i].position);
        if (distance < 0.02f)
        {
            i++;  // Move to the next point
            if (i >= points.Length)  // Check if 'i' has exceeded the bounds of the array
            {
                i = 0;  // Reset 'i' to the beginning
            }
        }

        // Calculate the dynamic speed based on the distance
        float speed = Mathf.Lerp(minSpeed, maxSpeed, distance / 0.5f); // Adjust the denominator to control the 'ease out' effect
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Ensure speed does not drop below minSpeed or exceed maxSpeed

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision){

        collision.transform.SetParent(transform); 

        }

    private void OnCollisionExit2D(Collision2D collision){

        collision.transform.SetParent(null); 
        
    }
}
