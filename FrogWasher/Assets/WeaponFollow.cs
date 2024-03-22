using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform target;
    private readonly float xOffset = 0.21f;
    private readonly float yOffset = -0.03f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(target.position.x + xOffset, target.position.y + yOffset);
        
    }
}
