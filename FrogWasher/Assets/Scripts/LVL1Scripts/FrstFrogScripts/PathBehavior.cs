using UnityEngine;
using System.Collections;

public class enemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public float originalSpeed;
    public float speed;
    private Transform currentPoint;
    public bool canMove = true;
    public Animator anim; 
    public GameObject player; 
    private bool originalDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        originalSpeed = speed; 
        player = GameObject.FindGameObjectWithTag("Player");
         originalDirection = transform.localScale.x > 0;
    }

    void Update()
    {
        CheckPlayerDistance();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("tongueAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("groundDeath"))
        {
            canMove = false;
        }
        else if (!anim.GetBool("IsAttacking"))
        {
            canMove = true;
        }

        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        else{
            Vector2 point = currentPoint.position - transform.position;
            rb.velocity = currentPoint == pointB.transform ? new Vector2(speed, 0) : new Vector2(-speed, 0);

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
            {
                flip();
                currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
            }
        }

    }

    private void flip()
    {
        originalDirection = !originalDirection;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void CheckPlayerDistance()
    {
        if (player == null) return; 
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 1.8f)
        {
            FacePlayer();
            anim.SetBool("IsAttacking", true);
        }
        else
        {
            RestoreOriginalDirection();
            anim.SetBool("IsAttacking", false);
        }
    }
        
    private void FacePlayer()
    {
        if (player.transform.position.x < transform.position.x && transform.localScale.x > 0 || 
            player.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            flip(); 
        }
    }
    private void RestoreOriginalDirection()
    {
    
        if (rb.velocity.x > 0 && transform.localScale.x < 0)
        {
            flip();
        }
        else if (rb.velocity.x < 0 && transform.localScale.x > 0) 
        {
            flip();
        }
    }

}
