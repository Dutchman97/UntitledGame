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

            Vector3 objectPosition = this.focusObject.transform.position;

            // Calculate the offset and position of the camera in world space.
            Vector3 offset = this.transform.rotation * this.relativeOffset;
            Vector3 position = objectPosition + offset;

            // Raycast variables.
            float maxDistance = offset.magnitude;
            RaycastHit raycastHit;
            Ray ray = new Ray(objectPosition, position - objectPosition);
            // Clipping through entities and the player is allowed.
            int layerMask = ~(1 << LayerMask.NameToLayer("Entities") | 1 << LayerMask.NameToLayer("Player"));

            // Check if something is in the way of the camera, and if so, move it.
            if (Physics.Raycast(ray, out raycastHit, maxDistance, layerMask)) {
                position = ray.GetPoint(raycastHit.distance * 0.9f);
            }

            // Set the camera's position to the focus object offset by a vector.
            this.transform.position = position;
        }
    }

    public Ray GetAimRay() {
        Vector3 direction = this.transform.forward;
        // The origin of the ray starts a bit forward.
        // This is done so that the ray starts in the same z-plane (of the camera's coordinate system) as the focus object.
        // If this would not be done, the aim ray can intersect with an object behind the focus object, which is not what we want.
        Vector3 origin = this.transform.position + direction * -this.relativeOffset.z;
        return new Ray(origin, direction);
    }
}
