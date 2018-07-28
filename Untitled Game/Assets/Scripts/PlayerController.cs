using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Object References")]
    [Tooltip("Camera that will be used for determining movement direction")]
    public Camera mainCamera;
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
        //Application.targetFrameRate = 5;
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
            Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, movement);
            this.legs.SetRotation(movementRotation);
        }

        // Calculate the point the player should be aiming at if any such point exists.
        RaycastHit raycastHit;
        bool raycastSuccess = Physics.Raycast(this.mainCamera.transform.position, this.mainCamera.transform.forward, out raycastHit);

        // Set the guns' rotation.
        if (raycastSuccess) {
            Vector3 cameraFocusPoint = raycastHit.point;
            this.leftGun.LookAt(cameraFocusPoint);
            this.rightGun.LookAt(cameraFocusPoint);
        }
        else {
            Quaternion lookRotation = this.mainCamera.transform.rotation; //Quaternion.FromToRotation(Vector3.forward, this.mainCamera.transform.forward);
            this.leftGun.SetRotation(lookRotation);
            this.rightGun.SetRotation(lookRotation);
        }

        // Calculate the camera rotation and sets the base's rotation to (a modification of) it.
        Quaternion baseRotation = this.mainCamera.transform.rotation;
        Vector3 baseRotationEuler = baseRotation.eulerAngles;
        Quaternion baseRotation1 = Quaternion.AngleAxis(baseRotationEuler.y, Vector3.up) * Quaternion.AngleAxis(baseRotationEuler.x, Vector3.right) * Quaternion.AngleAxis(baseRotationEuler.z, Vector3.forward);
        this.@base.SetRotation(baseRotation1);
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

    private void SetObjectRotation(Transform objectTransform, Quaternion rotation) {
        // Set the player's rotation.
        TargetRotation tr = objectTransform.GetComponent<TargetRotation>();
        if (tr == null) {
            objectTransform.transform.rotation = rotation;
        }
        else {
            tr.SetTargetRotation(rotation);
        }
    }
}
