using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileRender : MonoBehaviour
{
    public Transform target;
    public float distance;
   
    private LineRenderer lr;
    private readonly float xOffset = .12f;
    private readonly float yOffset = .03f;
    private readonly float width = .03f;
    private bool isIncreasing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        isIncreasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 startPos = new(target.position.x + xOffset, target.position.y + yOffset);
        Vector2 newPos = new(target.position.x + xOffset + distance, target.position.y + yOffset);
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, newPos);
        if (Input.GetMouseButtonDown(0)) // TODO: Change to default fire for controller compatability
        {
            if (!isIncreasing)
            {
                StartCoroutine(FireWeapon());
                isIncreasing = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isIncreasing = false;
        }
    }

    private IEnumerator FireWeapon()
    {
        float duration = .5f;
        float elapsedTime = 0f;
        float maxDistance = 0.5f;
        while (elapsedTime < duration)
        {
            distance = Mathf.Lerp(0f, maxDistance, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        distance = maxDistance;

        while (Input.GetMouseButton(0))
        {
            yield return null;
        }

        while (distance > 0)
        {
            distance -= Time.deltaTime * maxDistance;
            yield return null;
        }

        distance = 0f;
    }
}
