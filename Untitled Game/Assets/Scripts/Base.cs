using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    public float rotationLerpSpeed = 3.0f;

    private Quaternion rotationOffset, rotationTarget;

    private void Start() {
        this.rotationTarget = this.rotationOffset = this.transform.rotation;
    }

    private void FixedUpdate() {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.rotationTarget, this.rotationLerpSpeed * Time.fixedDeltaTime);
    }

    public void SetRotation(Quaternion rotation) {
        this.rotationTarget = rotation * this.rotationOffset;
        //this.transform.rotation = rotation * this.rotationOffset;
    }
}
