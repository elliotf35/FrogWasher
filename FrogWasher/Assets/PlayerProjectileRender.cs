using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerProjectileRender : MonoBehaviour
{
    private readonly float width = .02f;
    private bool left = false;
    // Start is called before the first frame update
    private float maxDistance = 0.5f; // Maximum distance the line should be drawn

    private LineRenderer lineRenderer;
    private Vector3 mousePosition;

    public int water = 1000;
    public int refillTimeout = 0;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    void Update()
    {
        left = transform.parent.rotation.y != 0;
        if (Input.GetButton("Fire1") && water > 0) {
            // Debug.Log("Firing");
            // Get the mouse position in world coordinates
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Make sure the z-coordinate is zero

            // Calculate the direction from the object to the mouse
            Vector3 direction = mousePosition - transform.position;
            

            // Clamp the magnitude of the direction to the maximum distance
            direction = direction.normalized * maxDistance;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            if (left) {
                if (angle > -90 && angle < 0) {
                direction.x = 0;
                direction.y = -maxDistance;
                }
                if (angle < 90 && angle >= 0) {
                    direction.x = 0;
                    direction.y = maxDistance;
                }
            } else {
                if (angle < -90) {
                direction.x = 0;
                direction.y = -maxDistance;
                }
                if (angle > 90) {
                    direction.x = 0;
                    direction.y = maxDistance;
                }
            }
            
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance);

            // If a collision is detected, adjust the line's end position
            if (hit.collider != null)
            {
                // Adjust the line's end position to the hit point
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {

                // Set the positions of the line renderer
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + direction);
            }
            water--;
            refillTimeout = 300;
            
        } else {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            if (water < 1000 && refillTimeout == 0) {
                water++;
            }
            if (refillTimeout > 0) {
                refillTimeout--;
            }

        }
        
    }
}
