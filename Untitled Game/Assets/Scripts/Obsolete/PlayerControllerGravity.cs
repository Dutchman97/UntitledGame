using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGravity : MonoBehaviour {
    [Header("Global References")]
    [Tooltip("Camera that will be used for determining movement direction")]
    public Camera mainCamera;

    public GameObject axisObject;

    [Header("Movement")]
    [Range(0.0f, float.MaxValue)]
    [Tooltip("Movement speed in units per second")]
    public float movementSpeed = 2.0f;
    
    private Vector3 gravity;

    private void Start() {
        // Check if a camera is referenced.
        // This is necessary to ensure the player object always moves and looks in the direction the camera is pointing.
        if (this.mainCamera == null) {
            Debug.Log("Error: no main camera set for PlayerController", this);
            Application.Quit();
        }

        this.gravity = Vector3.down;
    }

    private void Update() {
        CoordinateSystem cs = this.GetCurrentCoordinateSystem();

        // Get the player's movement vector.
        Vector3 movement = this.GetMovement(cs.X, cs.Y, cs.Z);

        // Move the player.
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + movement);

        Vector3 targetRotation = new Vector3(this.transform.rotation.eulerAngles.x, this.mainCamera.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        TargetRotation tr = this.GetComponent<TargetRotation>();
        tr.SetTargetRotation(targetRotation);

        this.axisObject.transform.position = this.transform.position;
        this.axisObject.transform.Find("X").localPosition = cs.X;
        this.axisObject.transform.Find("Y").localPosition = cs.Y;
        this.axisObject.transform.Find("Z").localPosition = cs.Z;

        // For sphere gravity
        this.gravity = -rb.position.normalized;

        // Let gravity affect the player.
        rb.AddForce(this.gravity * rb.mass, ForceMode.Force);

        if (Input.GetKey(KeyCode.Keypad7)) {
            this.gravity = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.Keypad9)) {
            this.gravity = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.Keypad8)) {
            this.gravity = Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.Keypad5)) {
            this.gravity = Vector3.back;
        }
        else if (Input.GetKey(KeyCode.Keypad4)) {
            this.gravity = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.Keypad6)) {
            this.gravity = Vector3.right;
        }
        else if (Input.GetKey(KeyCode.Keypad2)) {
            this.gravity = new Vector3(0.0f, -2.0f, -1.0f).normalized;
        }
        else if (Input.GetKey(KeyCode.Keypad0)) {
            this.gravity = Random.onUnitSphere;
        }

        //this.axisObject.transform.localScale = new Vector3(cs.X.magnitude, cs.Y.magnitude, cs.Z.magnitude);
    }

    /// <summary>
    /// Gets the movement vector of the player in world space, scaled by deltaTime
    /// </summary>
    /// <param name="x">The x-axis of the player's space</param>
    /// <param name="y">The y-axis of the player's space</param>
    /// <param name="z">The z-axis of the player's space</param>
    /// <returns>The movement vector of the player in world space</returns>
    private Vector3 GetMovement(Vector3 x, Vector3 y, Vector3 z) {
        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            movement += z;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement += -z;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement += -x;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += x;
        }
        return movement * this.movementSpeed * Time.deltaTime;
    }

    public CoordinateSystem GetCurrentCoordinateSystem() {
        // The player's y-axis, which goes in the opposite direction of gravity.
        Vector3 y = -this.gravity;

        // The player's x-axis, which should be the cross product of the player's up vector and the camera's forward vector.
        Vector3 x = Vector3.Cross(y, this.mainCamera.transform.forward).normalized;

        // The player's z-axis, which should be the cross product of the player's right and up vectors.
        // This could've also been the camera's forward vector,
        // but we want the player's forward vector to be orthogonal to gravity.
        Vector3 z = Vector3.Cross(x, y);

        return new CoordinateSystem(x, y, z, this.transform.position);
    }
}
