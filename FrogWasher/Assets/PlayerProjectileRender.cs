using UnityEngine;

public class PlayerProjectileRender : MonoBehaviour
{
    private readonly float width = .25f;
    private readonly float jetpackWidth = .5f; // Width for the jetpack stream
    private float maxDistance = 2f; // Maximum distance the line should be drawn
    private float jetpackDistance = 1f; // Shorter distance for the jetpack stream

    private LineRenderer lineRenderer;
    private Vector3 mousePosition;
    public GameObject character; // Add a public reference to your character

    private Rigidbody2D characterRigidbody; // Rigidbody2D reference for the character

    public int maxWater = 1000;
    public int water;
    public int refillTimeout = 0;
    public Vector2 secondPoint;
    public bool firing;

    public WaterBar waterBar;
    public float jetpackForce = 10f; // Control the strength of the jetpack

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        water = maxWater;
        waterBar.SetMaxAmmo(water);

        if (character != null) // Make sure the character is assigned
        {
            characterRigidbody = character.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D from the character
            if (characterRigidbody == null)
            {
                Debug.LogError("Missing Rigidbody2D on the character object.");
            }
        }
        else
        {
            Debug.LogError("Character GameObject is not assigned in PlayerProjectileRender.");
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && water > 0)
        {
            UseWaterAsProjectile();
        }
        else if (Input.GetButton("Fire2") && water > 0 && characterRigidbody != null)
        {
            UseWaterAsJetpack();
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

    private void UseWaterAsProjectile()
    {
        firing = true;
        SetProjectileVisuals(width, width); // Reset to default width for normal shooting
        ShootProjectile(maxDistance);
    }

    private void UseWaterAsJetpack()
    {
        firing = true;
        SetProjectileVisuals(jetpackWidth, jetpackWidth); // Wider and thicker for jetpack

        // Correctly calculate the mouse position like you do for shooting
        mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); // Correct depth calculation
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the gun to the mouse and invert it for jetpack propulsion
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Apply force in the opposite direction of where the gun is pointing
        characterRigidbody.AddForce(-direction * jetpackForce, ForceMode2D.Impulse);

        ShootProjectile(jetpackDistance); // Use a shorter distance for the jetpack

        water -= 1;
        refillTimeout = 300;
    }


    private void ShootProjectile(float distance)
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); // Ensure the z-depth is calculated correctly
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (mousePosition - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            // Debug to check what is being hit
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            Frog frog = hit.collider.GetComponent<Frog>();
            if (frog != null)
            {
                Debug.Log("Frog hit, dealing damage.");
                frog.TakeDamage(5);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * distance);
        }
        water--;
        refillTimeout = 300;
    }


    private void SetProjectileVisuals(float startWidth, float endWidth)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
    }
}
