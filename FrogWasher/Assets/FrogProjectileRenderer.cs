using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogProjectileRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private readonly float width = 0.02f;
    public bool playerInRange = false;
    private bool firing = false;

    private float yOffset = 0.03f;
    private float initialXOffset = -0.1f;
    private float range = -0.6f;
    
    private Vector2 retractedPosition;
    private Vector2 extendedPosition;
    private Frog frog;
    private float extendDuration = 1f;
    private Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        frog = transform.parent.GetComponent<Frog>();
        playerScript = frog.player.GetComponent<Player>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        retractedPosition = new(transform.parent.position.x + initialXOffset, transform.parent.position.y + yOffset);
        extendedPosition = new(transform.parent.position.x + range, transform.parent.position.y + yOffset);
        lineRenderer.SetPosition(0, retractedPosition);
        lineRenderer.SetPosition(1, retractedPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frog.playerInRange && !firing){
            StartCoroutine(Fire());
        }
        if (frog.health <= 0){
            StopAllCoroutines();
            lineRenderer.SetPosition(0, retractedPosition);
            lineRenderer.SetPosition(1, retractedPosition);
        }
        PlayerHit();
    }

    void PlayerHit(){
        Vector2 playerPosition = frog.player.transform.position;
        if (playerPosition.x < retractedPosition.x && playerPosition.x+0.05f > lineRenderer.GetPosition(1).x && playerPosition.y > retractedPosition.y - 0.3f && playerPosition.y < retractedPosition.y + 0.3f){
            playerScript.PlayerHit();
        }
    }

    private IEnumerator Fire(){
        firing = true;
        float elapsedTime = 0f;

        // Extend line
        while (elapsedTime < extendDuration)
        {
            float t = elapsedTime / extendDuration;
            lineRenderer.SetPosition(1, Vector3.Lerp(retractedPosition, extendedPosition, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the line reaches its endpoint precisely
        lineRenderer.SetPosition(1, extendedPosition);

        // Wait for a short time before retracting (optional)
        yield return new WaitForSeconds(0.5f);

        // Retract line
        while (elapsedTime > 0)
        {
            float t = elapsedTime / extendDuration;
            lineRenderer.SetPosition(1, Vector3.Lerp(retractedPosition, extendedPosition, t));
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        // Ensure the line returns to its starting point precisely
        lineRenderer.SetPosition(1, retractedPosition);
        yield return new WaitForSeconds(0.5f);
        firing = false;
    }
}
