using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this value to control the camera movement speed
    public float boundary = 25f; // Adjust this value to set the distance from the screen edge that triggers movement
    public Transform player;

    void Update()
    {
        // Get mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Check if mouse is near the edges of the screen
        if (mousePosition.x > boundary || mousePosition.x < Screen.width - boundary ||
            mousePosition.y > boundary || mousePosition.y < Screen.height - boundary)
        {
            // Calculate movement based on mouse input
            float horizontalMovement = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float verticalMovement = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;

            // Move the camera
            transform.Translate(new Vector3(horizontalMovement, verticalMovement, 0));
        }

        // Move the camera back to its original position
        float x = transform.position.x;
        float y = transform.position.y;

        x = Mathf.Lerp(x, player.position.x, .04f);
        y = Mathf.Lerp(y, player.position.y, .04f);

        transform.position = new Vector3(x, y, -10f);
    }
}
