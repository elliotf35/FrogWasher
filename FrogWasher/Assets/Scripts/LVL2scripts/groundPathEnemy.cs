using UnityEngine;
using System.Collections;

public class enemyPatrol2 : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public float speed;
    private Transform currentPoint;
    public bool canMove = true;
    public Animator anim;
    public GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateDirection();  // Ensure correct initial direction
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("groundDeath"))
        {
            canMove = false;
        }

        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 point = currentPoint.position - transform.position;
        rb.velocity = currentPoint == pointB.transform ? new Vector2(speed, 0) : new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
            UpdateDirection();
        }
    }

    private void UpdateDirection()
    {
        // Set direction based on the next point
        float direction = currentPoint == pointB.transform ? -1f : 1f;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }
}
