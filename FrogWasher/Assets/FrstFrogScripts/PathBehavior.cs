using UnityEngine;
using System.Collections;

public class enemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    public float originalSpeed;
    public float speed;
    private Transform currentPoint;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        originalSpeed = speed; // Initialize and store the original speed
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 point = currentPoint.position - transform.position;
        rb.velocity = currentPoint == pointB.transform ? new Vector2(speed, 0) : new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            flip();
            currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        float targetSpeed = originalSpeed * slowFactor; // Calculate target speed
        if (speed > targetSpeed)  // Only apply slow if it results in a lower speed than currently set
            speed = targetSpeed;
        
        StartCoroutine(RestoreSpeed(duration));
    }
    
    IEnumerator RestoreSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed; // Restore the original speed
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
