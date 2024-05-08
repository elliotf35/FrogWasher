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

    void Start()
    {
        transform.position = points[startingPoint].position;
        i = startingPoint;  // Ensure 'i' starts at 'startingPoint'
        InitializeMinions();  // Initially deactivate all minions
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, points[i].position);
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
                i = 0;
            }
        }

        // Calculate the dynamic speed based on the distance
        float speed = Mathf.Lerp(minSpeed, maxSpeed, distance / 0.5f); // Adjust the denominator to control the 'ease out' effect
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Ensure speed does not drop below minSpeed or exceed maxSpeed

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
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
