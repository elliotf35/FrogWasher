using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPath : MonoBehaviour
{
    public float maxSpeed;
    public int startingPoint;
    public Transform[] points;
    private int i;
    private float minSpeed = 0.1f; // Minimum speed to avoid the platform stopping completely before it reaches the point
    public GameObject[] minions; // Array to hold references to the minion GameObjects
    public GameObject[] superMinions; 
    public bool canActivateSuperMinions = false;

    public bool canActivateMinions = true;
    private bool shouldMoveToStart = true;

    void Start()
    {
    
        i = startingPoint;  // Ensure 'i' starts at 'startingPoint'
        InitializeMinions();  // Initially deactivate all minions
        StartCoroutine(MoveToStartPoint(points[startingPoint].position));
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, points[i].position);
        if (!shouldMoveToStart)
        {
            if (distance < 0.02f)
            {
                if (canActivateMinions)
                {
                    ActivateMinion(i);
                }

                if (canActivateSuperMinions)
                {
                    ActivateSuperMinion(i);
                }
                i++;
                if (i >= points.Length)
                {
                    i = 0;  // Loop back to the start of the array
                }
            }
        }

        // Calculate the dynamic speed based on the distance
        if (!shouldMoveToStart) // Ensure this runs only when boss is moving between points
        {
            float speed = Mathf.Lerp(minSpeed, maxSpeed, distance / 0.5f); // Adjust the denominator to control the 'ease out' effect
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Ensure speed does not drop below minSpeed or exceed maxSpeed

            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    IEnumerator MoveToStartPoint(Vector3 startPosition)
    {
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, maxSpeed * Time.deltaTime);
            yield return null;
        }
        shouldMoveToStart = false;  // Stop the initial movement and start regular pathing
    }

    private void InitializeMinions()
    {
        foreach (GameObject minion in minions)
        {
            minion.SetActive(false);
        }
    }

    private void ActivateMinion(int index)
    {
        if (index < minions.Length && !minions[index].activeSelf)
        {
            minions[index].transform.position = transform.position;
            minions[index].SetActive(true);
        }
    }

    private void ActivateSuperMinion(int index)
    {
        if (index < superMinions.Length && !superMinions[index].activeSelf)
        {
            superMinions[index].transform.position = transform.position;
            superMinions[index].SetActive(true);
        }
    }

    public void StopActivatingMinions()
    {
        canActivateMinions = false;
    }

    public void StopActivatingSuperMinions()
    {
        canActivateSuperMinions = false;
    }
}
