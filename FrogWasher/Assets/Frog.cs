using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Frog : MonoBehaviour   
{

    public readonly float initHealth = 100;
    public float health;
    
    private PlayerProjectileRender ppr;
    public GameObject player;
    public GameObject pwt;
    private Vector2 point;
    private bool playerFiring;
    public Vector2 healthBar0;
    public Vector2 healthBar1;
    private float distanceToPlayer;
    private BoxCollider2D bc;
    private Rigidbody2D rb2d;
    public bool playerInRange;


    // Start is called before the first frame update
    void Start()
    {
        health = initHealth;
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        bc = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        point = ppr.secondPoint;
        playerFiring = ppr.firing;
        if (playerFiring && IsWithin(point)) health -= 1;
        
        if (health <= 0) {
            rb2d.constraints = RigidbodyConstraints2D.None;
            bc.enabled = false;
        }
        
        if (distanceToPlayer <= 1.0f){
            playerInRange = true;
        } else {
            playerInRange = false;
        }


    }

    public bool IsWithin(Vector2 point)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Collider2D component not found.");
            return false;
        }

        // Get the closest point on the collider to the given point
        Vector2 closestPoint = collider.ClosestPoint(point);

        // Calculate the distance between the given point and the closest point on the collider
        float distance = Vector2.Distance(point, closestPoint);

        // Check if the distance is within the threshold
        return distance <= 0;
    }
    
}
