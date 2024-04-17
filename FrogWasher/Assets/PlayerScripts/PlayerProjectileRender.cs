using UnityEngine;
using System.Collections;

public class PlayerProjectileRender : MonoBehaviour
{
    private readonly float width = .25f;
    private readonly float jetpackWidth = .5f; // Width for the jetpack stream
    private float maxDistance = 1.4f; // Maximum distance the line should be drawn
    private float jetpackDistance = 1f; // Shorter distance for the jetpack stream

    private LineRenderer lineRenderer;
    private Vector3 mousePosition;
    public GameObject character; // Add a public reference to your character
    public GameObject powerWasherTip;
    private Rigidbody2D characterRigidbody; // Rigidbody2D reference for the character

    public int maxWater = 1000;
    public int water;
    public int refillTimeout = 0;
    public Vector2 secondPoint;
    public bool firing;
    private Animator powerWasherAnimator;
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
        powerWasherAnimator = powerWasherTip.GetComponent<Animator>();
        if (powerWasherAnimator == null)
        {
            Debug.LogError("Missing Animator on the PowerWasherTip object.");
        }

    }

    void Update()
    {
        // Reset shooting and jetpacking animations when not active
        if (!Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            if (powerWasherAnimator != null)
            {
                powerWasherAnimator.SetBool("IsJetPacking", false);
                powerWasherAnimator.SetBool("IsShooting", false);
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        if (Input.GetButton("Fire1") && water > 0)
        {
            UseWaterAsProjectile();
        }
        else if (Input.GetButton("Fire2") && water > 0 && characterRigidbody != null)
        {
            UseWaterAsJetpack();
        }

        // Manage water and ammo UI
        if (refillTimeout > 0)
            refillTimeout--;
        if (water < maxWater && refillTimeout == 0)
            water += 18;

        waterBar.SetAmmo(water);
    }

    private void UseWaterAsProjectile()
    {
        firing = true;
        SetProjectileVisuals(width, width); // Reset to default width for normal shooting
        if (powerWasherAnimator != null) 
        {
            powerWasherAnimator.SetBool("IsJetPacking", false); // Ensure jetpack is not flagged
            powerWasherAnimator.SetBool("IsShooting", true);   // Flag shooting as active
        }
        ShootProjectile(maxDistance);
    }

    private void UseWaterAsJetpack()
    {
        firing = true;
        SetProjectileVisuals(jetpackWidth, jetpackWidth); // Wider and thicker for jetpack
        if (powerWasherAnimator != null) 
        {
            powerWasherAnimator.SetBool("IsJetPacking", true);
            powerWasherAnimator.SetBool("IsShooting", false); // Ensure shooting is not flagged
        }

        // Correctly calculate the mouse position like you do for shooting
        mousePosition = Input.mousePosition;
        // Important: Adjust the Z position to be the distance from the camera to the character
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the character to the mouse position
        Vector3 direction = (new Vector3(mousePosition.x, mousePosition.y, transform.position.z) - transform.position).normalized;

        // Apply force in the opposite direction of where the mouse is pointing
        characterRigidbody.AddForce(-direction * jetpackForce, ForceMode2D.Impulse);

        ShootProjectile(jetpackDistance); // Use a shorter distance for the jetpack

        if (powerWasherAnimator != null) 
        {
            powerWasherAnimator.SetBool("IsJetPacking", true);
        }

        water -= 4;
        refillTimeout = 300;
        if (water <= 0 && powerWasherAnimator != null) powerWasherAnimator.SetBool("IsJetPacking", false);
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
                Vector2 damageDirection = (hit.point - new Vector2(transform.position.x, transform.position.y)).normalized;
                frog.TakeDamage(5, damageDirection);
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
