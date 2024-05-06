using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    public float maxSpeed;
    public Transform[] points;
    private int targetIndex = 1;
    private float minSpeed = 0.1f;
    private Vector3 lastPosition;
    private bool shouldSetParent = false; // Flag to indicate if the parent should be set

    void Start()
    {
        transform.position = points[0].position;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (points.Length < 2) return;

        float distance = Vector2.Distance(transform.position, points[targetIndex].position);

        float speed = Mathf.Lerp(minSpeed, maxSpeed, distance / 0.5f);
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        Vector3 currentPosition = transform.position;

        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position, speed * Time.deltaTime);

        // Update direction based on movement
        float direction = Mathf.Sign(transform.position.x - lastPosition.x);

        // Check if direction changed and flip if necessary
        if (direction != 0 && direction != Mathf.Sign(transform.localScale.x))
        {
            flip();
        }

        lastPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shouldSetParent = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (shouldSetParent)
        {
            collision.transform.SetParent(transform);
            shouldSetParent = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
