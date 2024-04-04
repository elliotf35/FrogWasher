using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Frog : MonoBehaviour   
{

    public int health =  300;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    private Vector2 point;
    private bool firing;
    // Start is called before the first frame update
    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
    }

    // Update is called once per frame
    void Update()
    {
        point = ppr.secondPoint;
        firing = ppr.firing;
        if (firing && IsWithin(point)){
            health -= 1;
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
