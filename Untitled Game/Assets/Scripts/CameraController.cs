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

    [Tooltip("Determine whether the rotation around the x-axis should be clamped or not")]
    public bool clampXRotation = true;

    private Vector2 mouseMovement = Vector2.zero;

    private void Update() {
        // Get the mouse movement.
        this.mouseMovement += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (this.invertY ? 1 : -1)) * this.mouseSensitivity;

        // Clamp the total y-axis mouse movement.
        this.mouseMovement.y = Mathf.Clamp(this.mouseMovement.y, -80.0f, 80.0f);

        if (this.focusObject != null) {
            PlayerController pc = this.focusObject.GetComponent<PlayerController>();

            // Camera is currently only intended for the player's object.
            if (pc == null) {
                Debug.LogWarning("The focus object does not contain a PlayerController. The camera is currently only supported for following the player object.");
                return;
            }

            // Set the camera's rotation.
            this.transform.localRotation = Quaternion.identity;
            this.transform.Rotate(Vector3.up, this.mouseMovement.x, Space.World);
            this.transform.Rotate(this.transform.right, this.mouseMovement.y, Space.World);

            // Calculate the offset in world space.
            Vector3 offset = this.transform.rotation * this.relativeOffset;
            
            // Set the camera's position to the focus object offset by a vector.
            this.transform.position = this.focusObject.transform.position + offset;
        }
    }
}
