using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Object References")]
    [Tooltip("Camera that will be used for determining movement direction")]
    public CameraController mainCamera;
    [Tooltip("The player's legs component")]
    public Legs legs;
    [Tooltip("The player's base component")]
    public Base @base;
    [Tooltip("The player's left gun component")]
    public Gun leftGun;
    [Tooltip("The player's right gun component")]
    public Gun rightGun;

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

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    private void Update() {
        // Derive the forward vector from the camera's forward vector.
        Vector3 movementForward = Vector3.ProjectOnPlane(this.mainCamera.transform.forward, Vector3.up).normalized;

        // Get the player's movement vector.
        Vector3 movement = this.GetMovement(movementForward);

        // Move the player.
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + movement);

        // Calculate the movement rotation and set the legs' rotation to it.
        if (movement != Vector3.zero) {
            Quaternion movementRotation = Quaternion.LookRotation(movement, Vector3.up);
            this.legs.SetRotation(movementRotation);
        }

        // Calculate the point the player should be aiming at if any such point exists.
        RaycastHit raycastHit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Player")); // Do not aim at the player.
        bool raycastSuccess = Physics.Raycast(this.mainCamera.GetAimRay(), out raycastHit, float.MaxValue, layerMask);

        // Set the guns' rotation.
        if (raycastSuccess) {
            Vector3 cameraFocusPoint = raycastHit.point;
            this.leftGun.LookAt(cameraFocusPoint);
            this.rightGun.LookAt(cameraFocusPoint);
        }
        else {
            Quaternion lookRotation = this.mainCamera.transform.rotation;
            this.leftGun.SetRotation(lookRotation);
            this.rightGun.SetRotation(lookRotation);
        }

        // Make the base look in the same direction as the camera.
        this.@base.LookTowardCameraDirection(this.mainCamera.transform);
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
