using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerProjectileRender : MonoBehaviour
{
    public Transform target;
    public float distance;
   
    private LineRenderer lr;
    private float xOffset = .12f;
    private readonly float yOffset = .03f;
    private readonly float width = .03f;
    private bool isIncreasing = false;
    private bool left = false;
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
        float md = .5f;
        xOffset = 0.12f;
        // if (target.rotation.y == 1){
        //     if (!left) {
        //         left = true;
        //         StopAllCoroutines();
        //     }
        //     xOffset = -0.12f;
        //     md = -0.5f;
            
        // } else {
        //     left = false;
        // }
        Vector2 startPos = new(target.position.x + xOffset, target.position.y + yOffset);
        Vector2 newPos = new(target.position.x + xOffset + distance, target.position.y + yOffset);
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, newPos);
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isIncreasing)
            {
                StopAllCoroutines();
                StartCoroutine(FireWeapon(md, left));
                isIncreasing = true;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isIncreasing = false;
        }
    }

    private IEnumerator FireWeapon(float maxDistance, bool l)
    {
        float duration = .5f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            distance = Mathf.Lerp(0f, maxDistance, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        distance = maxDistance;

        while (Input.GetButton("Fire1"))
        {
            yield return null;
        }

        while (distance > 0)
        {
            // if (left) {
            //     distance += Time.deltaTime * Math.Abs(maxDistance);
            // } else {
            //     distance -= Time.deltaTime * Math.Abs(maxDistance);
            // }
            distance -= Time.deltaTime * Math.Abs(maxDistance);
            yield return null;
        }
        // while (elapsedTime > 0){
        //     distance = Mathf.Lerp(maxDistance, 0f, elapsedTime / duration);
        //     elapsedTime -= Time.deltaTime;
        //     yield return null;
        // }

        distance = 0f;
    }
}
