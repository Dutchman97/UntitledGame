using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public Transform boneTransform;
    public bool rightGun;
    public float rotationLerpSpeed = 2.0f;

    private Quaternion rotationOffset, rotationTarget;

    private void Start() {
        this.rotationTarget = this.rotationOffset = this.boneTransform.rotation;
    }

    private void FixedUpdate() {
        this.boneTransform.rotation = Quaternion.Lerp(this.boneTransform.rotation, this.rotationTarget, this.rotationLerpSpeed * Time.fixedDeltaTime);
    }

    public void SetRotation(Quaternion rotation) {
        this.rotationTarget = rotation * this.rotationOffset;
    }

    public void LookAt(Vector3 point) {
        Vector3 lookDirection = point - this.boneTransform.position;
        Quaternion lookRotation = Quaternion.FromToRotation(Vector3.forward, lookDirection);
        this.SetRotation(lookRotation);
    }
}
