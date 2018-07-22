using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerGravity : MonoBehaviour {
    [Tooltip("Object the camera will focus on")]
    public GameObject focusObject;

    [Tooltip("Distance the camera will be from the object")]
    [Range(0.0f, float.MaxValue)]
    public float distanceToObject = 7.5f;

    [Tooltip("Mouse sensitivity")]
    public float mouseSensitivity = 5.0f;

    [Tooltip("Invert Y-axis rotation")]
    public bool invertY = false;

    private Vector2 relativeRotation2D;

    private void Update() {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (this.invertY ? 1 : -1));
        mouseMovement *= this.mouseSensitivity;
        //this.relativeRotation2D += mouseMovement;

        if (this.focusObject != null) {
            //this.transform.rotation = Quaternion.identity;

            PlayerControllerGravity pc = this.focusObject.GetComponent<PlayerControllerGravity>();
            if (pc == null)
                return;

            CoordinateSystem cs = pc.GetCurrentCoordinateSystem();
            this.transform.up = cs.Y;
            //this.transform.forward = cs.Z;

            this.transform.Rotate(cs.Y, mouseMovement.x, Space.World);
            this.transform.Rotate(this.transform.right, mouseMovement.y, Space.World);
            //this.transform.Rotate(cs.Y, this.relativeRotation2D.x, Space.World);
            //this.transform.Rotate(this.transform.right, this.relativeRotation2D.y, Space.World);
            //this.transform.Rotate(0.0f, mouseMovement.x, 0.0f, Space.Self);
            //this.transform.Rotate(mouseMovement.y, 0.0f, 0.0f, Space.World);


            this.transform.position = this.focusObject.transform.position - this.transform.forward * this.distanceToObject;
            this.transform.position += cs.Y * 2.0f;
        }
    }
}
