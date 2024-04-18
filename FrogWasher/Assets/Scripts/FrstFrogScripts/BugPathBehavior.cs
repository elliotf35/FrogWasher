using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrolVertical : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;
    private float originalSpeed; // To store the original speed

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        originalSpeed = speed; // Store the original speed at start
    }

    void Update()
    {
        Vector2 movement = (currentPoint.position - transform.position).normalized * speed;
        rb.velocity = movement;

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            ToggleDirection();
        }
    }

    void ToggleDirection()
    {
        currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        StartCoroutine(SlowDown(slowFactor, duration));
    }

    IEnumerator SlowDown(float slowFactor, float duration)
    {
        float tempSpeed = speed;  
        speed *= slowFactor;
        Debug.Log("Speed slowed to: " + speed);  
        yield return new WaitForSeconds(duration);
        speed = tempSpeed;  
        Debug.Log("Speed restored to: " + speed);  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
    }
}
