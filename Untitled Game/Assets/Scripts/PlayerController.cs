using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Global References")]
    [Tooltip("Camera that will be used for determining movement direction")]
    public Camera mainCamera;
    
    [Header("Movement")]
    [Range(0.0f, float.MaxValue)]
    [Tooltip("Movement speed in units per second")]
    public float movementSpeed = 2.0f;

    private void Start() {
        // Check if a camera is referenced.
        // This is necessary to ensure the player object always moves and looks in the direction the camera is pointing.
        if (this.mainCamera == null) {
            Debug.Log("Error: no main camera set for PlayerController", this);
            Application.Quit();
        }
    }

    private void Update() {
        // Derive the forward vector from the camera's forward vector.
        Vector3 forward = Vector3.ProjectOnPlane(this.mainCamera.transform.forward, Vector3.up).normalized;

        // Get the player's movement vector.
        Vector3 movement = this.GetMovement(forward);

        // Move the player.
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + movement);

        // Set the player's rotation.
        TargetRotation tr = this.GetComponent<TargetRotation>();
        if (tr == null) {
            this.transform.forward = forward;
        }
        else {
            tr.SetTargetRotation(Quaternion.LookRotation(forward, Vector3.up));
        }
    }

    /// <summary>
    /// Gets the movement vector of the player, scaled by deltaTime.
    /// </summary>
    /// <param name="forward">The forward vector. Must be a unit vector.</param>
    /// <returns>The movement vector of the player.</returns>
    private Vector3 GetMovement(Vector3 forward) {
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            movement += forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement -= forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement -= right;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += right;
        }
        return movement * this.movementSpeed * Time.deltaTime;
    }
}
