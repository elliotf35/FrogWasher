using UnityEngine;

public class PlayerProjectileRender : MonoBehaviour
{
    private readonly float width = .25f;
    private float maxDistance = 2f; // Maximum distance the line should be drawn

    private LineRenderer lineRenderer;
    private Vector3 mousePosition;

    public int maxWater = 1000;
    public int water;

    public int refillTimeout = 0;
    public Vector2 secondPoint;
    public bool firing;

    public WaterBar waterBar;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        water = maxWater;
        waterBar.SetMaxAmmo(water);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && water > 0)
        {
            firing = true;

            // Properly calculate the mouse position
            mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); // Correct depth calculation
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate the direction from the gun to the mouse
            Vector3 direction = (mousePosition - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance);
            
            if (hit.collider != null)
            {
                // Adjust the line's end position to the hit point
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                // Set the positions of the line renderer to simulate the water path
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + direction * maxDistance); // Ensure the direction is extended to maxDistance
            }
            water--;
            refillTimeout = 300;
        }
        else
        {
            firing = false;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            if (refillTimeout > 0)
                refillTimeout--;
            if (water < maxWater && refillTimeout == 0)
                water++;
        }
        waterBar.SetAmmo(water);
    }
}
