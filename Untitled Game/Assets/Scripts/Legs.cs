using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour {
    public Transform boneTransform;
    public float rotationLerpSpeed = 4.0f;

    private Quaternion rotationOffset, rotationTarget;

    private void Start() {
        this.rotationTarget = this.rotationOffset = this.boneTransform.rotation;
    }

    private void FixedUpdate() {
        this.boneTransform.rotation = Quaternion.Slerp(this.boneTransform.rotation, this.rotationTarget, this.rotationLerpSpeed * Time.fixedDeltaTime);
    }

    public void SetRotation(Quaternion rotation) {
        this.rotationTarget = rotation * this.rotationOffset;
    }
}
