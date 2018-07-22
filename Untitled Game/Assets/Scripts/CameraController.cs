using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Tooltip("Object the camera will focus on")]
    public GameObject focusObject;

    [Tooltip("Offset the camera will have relative to the object")]
    public Vector3 relativeOffset = new Vector3(0.0f, 2.0f, -7.5f);

    [Tooltip("Mouse sensitivity")]
    public float mouseSensitivity = 5.0f;

    [Tooltip("Invert Y-axis rotation")]
    public bool invertY = false;

    private void Update() {
        // Get the mouse movement.
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (this.invertY ? 1 : -1));
        mouseMovement *= this.mouseSensitivity;

        if (this.focusObject != null) {
            PlayerController pc = this.focusObject.GetComponent<PlayerController>();

            // Camera is currently only intended for the player's object.
            if (pc == null) {
                Debug.LogWarning("The focus object does not contain a PlayerController. The camera is currently only supported for following the player object.");
                return;
            }

            // Adjust the camera's rotation.
            this.transform.Rotate(Vector3.up, mouseMovement.x, Space.World);
            this.transform.Rotate(this.transform.right, mouseMovement.y, Space.World);

            // Calculate the offset in world space.
            Vector3 offset = this.transform.rotation * this.relativeOffset;
            
            // Set the camera's position to the focus object offset by a vector.
            this.transform.position = this.focusObject.transform.position + offset;
        }
    }
}
