using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHealthBar : MonoBehaviour
{
    private Frog frog;
    private LineRenderer lineRenderer;
    public Vector2 healthBar0;
    public Vector2 healthBar1;
    private readonly float width = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        frog = transform.parent.GetComponent<Frog>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Two points: start and end
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        healthBar0 = new(transform.parent.position.x - .2f, transform.parent.position.y + .2f);
        healthBar1 = new(healthBar0.x + .4f, healthBar0.y);
        lineRenderer.SetPosition(0, healthBar0);
        lineRenderer.SetPosition(1, healthBar1);
    }

    // Update is called once per frame
    void Update()
    {
        float percent = frog.health / frog.initHealth;
        float offset = Mathf.Max(0, 0.4f * percent);
        healthBar1 = new(healthBar0.x + offset, healthBar0.y);
        lineRenderer.SetPosition(1, healthBar1);
    }
}
