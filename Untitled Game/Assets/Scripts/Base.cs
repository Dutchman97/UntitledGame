using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    public float rotationLerpSpeed = 3.0f;

    public float xAngleScale = 1.0f / 3.0f;

    private Quaternion rotationOffset, rotationTarget;

    private void Start() {
        this.rotationTarget = this.rotationOffset = this.transform.rotation;
    }

    private void FixedUpdate() {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.rotationTarget, this.rotationLerpSpeed * Time.fixedDeltaTime);
    }

    public void SetRotation(Quaternion rotation) {
        this.rotationTarget = rotation * this.rotationOffset;
    }

    public void LookTowardCameraDirection(Transform cameraTransform) {
        // Get the direction in which the player is looking.
        Vector3 lookDirection = cameraTransform.forward;

        // Project the look direction on the y-plane, so that we can get the look direction's angle with it.
        Vector3 yPlaneProjection = Vector3.Scale(lookDirection, new Vector3(1.0f, 0.0f, 1.0f));
        float lookAngleX = Vector3.Angle(lookDirection, yPlaneProjection);
        lookAngleX *= lookDirection.y >= 0.0f ? 1.0f : -1.0f;  // This is done so that when looking down, the angle will be negative.

        // Sets the base's rotation to the camera's rotation, with a scaled x-angle.
        Quaternion baseRotation = Quaternion.AngleAxis(lookAngleX * (1.0f - this.xAngleScale), cameraTransform.right) * cameraTransform.rotation;
        this.SetRotation(baseRotation);
    }
}
