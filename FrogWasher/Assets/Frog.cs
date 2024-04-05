using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Frog : MonoBehaviour   
{

    public float health = 300;
    private readonly float initHealth = 300;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    private Vector2 point;
    private bool firing;
    private LineRenderer lineRenderer;
    public Vector2 healthBar0;
    public Vector2 healthBar1;

    private readonly float width = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        healthBar0 = new(transform.position.x - .2f, transform.position.y + .2f);
        healthBar1 = new(healthBar0.x + .4f, healthBar0.y);
        lineRenderer.SetPosition(0, healthBar0);
        lineRenderer.SetPosition(1, healthBar1);
    }

    // Update is called once per frame
    void Update()
    {
        point = ppr.secondPoint;
        firing = ppr.firing;
        if (firing && IsWithin(point)){
            health -= 1;
            float percent = health / initHealth;
            float offset = Mathf.Max(0, 0.4f * percent);
            Debug.Log("Offset" + offset);
            Debug.Log("Percent" + percent);
            healthBar1 = new(healthBar0.x + offset, healthBar0.y);
            lineRenderer.SetPosition(1, healthBar1);
        }
        if (health <= 0) {
            BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
            bc.enabled = false;
        }
        Debug.Log(IsWithin(point));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts.ToString());
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
        return distance <= .1f;
    }
    
}
