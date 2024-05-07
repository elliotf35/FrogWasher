using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform target; // The character transform
    public float xOffset = 0.5f; // Horizontal offset from the character
    public float yOffset = -0.03f; // Vertical offset from the character

    void Update()
    {
        // Get the mouse position and convert it to world coordinates
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f; // Ensure mouse position is at correct depth relative to the camera
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the weapon to the mouse position
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        // Calculate the angle between the direction and the x-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Determine the weapon's orientation and offset based on the mouse position
        if (target.rotation.y == 1) // Mouse is left of the character
        {
            // Adjust the weapon to mirror vertically and flip the xOffset
            transform.rotation = Quaternion.Euler(180, 0, -angle);
            transform.position = new Vector3(
                target.position.x - xOffset, // Flipped xOffset to the left
                target.position.y + yOffset,
                transform.position.z
            );
        }
        else // Mouse is right of the character
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position = new Vector3(
                target.position.x + xOffset, // Normal xOffset to the right
                target.position.y + yOffset,
                transform.position.z
            );
        }
    }
}
