using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerProjectileRender : MonoBehaviour
{
    private readonly float width = .25f;
    private readonly float jetpackWidth = .5f; 
    private float maxDistance = 3.7f; 
    private float jetpackDistance = 1f; 

    private LineRenderer lineRenderer;
    private Vector3 mousePosition;
    public GameObject character; 
    public GameObject powerWasherTip;
    private Rigidbody2D characterRigidbody;

    public int maxWater = 1000;
    public int water;
    public int refillTimeout = 0;
    public Vector2 secondPoint;
    public bool firing;
    private Animator powerWasherAnimator;
    public WaterBar waterBar;
    public float jetpackForce = 10f;
    public float jetpackForceX = 0;

    private float velocityCap = 10f;
    public Transform king;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; 
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        water = maxWater;
        waterBar.SetMaxAmmo(water);

        if (character != null) 
        {
            characterRigidbody = character.GetComponent<Rigidbody2D>(); 
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
        if (!Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            if (powerWasherAnimator != null)
            {
                powerWasherAnimator.SetBool("IsJetPacking", false);
                powerWasherAnimator.SetBool("IsShooting", false);
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);

            jetpackForceX = 0;

            PlayerKnockback playerKnockBack = character.GetComponentInParent<PlayerKnockback>();
            if (playerKnockBack != null)
            {
                playerKnockBack.AddExternalForceX(jetpackForceX);
            }
        }

        if (Input.GetButton("Fire1") && water > 0)
        {
            UseWaterAsProjectile();
        }
        else if (Input.GetButton("Fire2") && water > 0 && characterRigidbody != null)
        {
            UseWaterAsJetpack();
        }

        if (refillTimeout > 0)
            refillTimeout--;
        if (water < maxWater && refillTimeout == 0)
            water += 18;

        waterBar.SetAmmo(water);
    }

    private void UseWaterAsProjectile()
    {
        firing = true;
        SetProjectileVisuals(width, width); 
        if (powerWasherAnimator != null) 
        {
            powerWasherAnimator.SetBool("IsJetPacking", false); 
            powerWasherAnimator.SetBool("IsShooting", true);   
        }
        ShootProjectile(maxDistance);

        if (water <= 0 && powerWasherAnimator != null) powerWasherAnimator.SetBool("IsShooting", false);
    }

    private void UseWaterAsJetpack()
    {
        firing = true;
        SetProjectileVisuals(jetpackWidth, jetpackWidth);
        if (powerWasherAnimator != null)
        {
            powerWasherAnimator.SetBool("IsJetPacking", true);
            powerWasherAnimator.SetBool("IsShooting", false);
        }

        mousePosition = Input.mousePosition;
        mousePosition.z = 10;  
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction2D = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        Vector2 forceToAdd = -direction2D * jetpackForce;

        
        Vector2 currentVelocity = characterRigidbody.velocity;
        Vector2 newVelocity = new Vector2(currentVelocity.x + forceToAdd.x, Mathf.Min(currentVelocity.y + forceToAdd.y, velocityCap));
        

        characterRigidbody.velocity = newVelocity;
        jetpackForceX = forceToAdd.x * 5;

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0)
        {
            jetpackForceX = jetpackForceX*(-1); 
        }

        PlayerKnockback playerKnockBack = character.GetComponentInParent<PlayerKnockback>();
        if (playerKnockBack != null)
        {
            playerKnockBack.AddExternalForceX(jetpackForceX);
        }

        ShootProjectile(jetpackDistance);

        if (powerWasherAnimator != null)
        {
            powerWasherAnimator.SetBool("IsJetPacking", true);
        }

        water -= 4;
        refillTimeout = 300;
        if (water <= 0 && powerWasherAnimator != null) powerWasherAnimator.SetBool("IsJetPacking", false);
        Debug.Log("Direction: " + direction2D + ", Force Applied: " + forceToAdd);
    }


    private void ShootProjectile(float distance)
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); 
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (mousePosition - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Hit: " + hit.collider.gameObject.name);

            Frog frog = hit.collider.GetComponent<Frog>();
            if (frog != null)
            {
                Debug.Log("Frog hit, dealing damage.");
                Vector2 damageDirection = (hit.point - new Vector2(transform.position.x, transform.position.y)).normalized;
                frog.TakeDamage(5, damageDirection);
            }

            Frog2 frog2 = hit.collider.GetComponent<Frog2>();
            if (frog2 != null)
            {
                Debug.Log("Frog2 hit, dealing damage.");
                Vector2 damageDirection = (hit.point - new Vector2(transform.position.x, transform.position.y)).normalized;
                frog2.TakeDamage(5, damageDirection);
            }

            Boss boss = hit.collider.GetComponent<Boss>();
            if (boss != null) 
            {
                Vector2 damageDirection = (hit.point - new Vector2(transform.position.x, transform.position.y)).normalized;
                boss.TakeDamage(2);  
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * distance);
        }
        water-= 2;
        refillTimeout = 300;
    }


    private void SetProjectileVisuals(float startWidth, float endWidth)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
    }
}
