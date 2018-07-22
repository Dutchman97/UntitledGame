using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour {
    public float lerpSpeed = 3f;

    public bool targetX = false, targetY = true, targetZ = false;

    private Vector3 targetRotation;

    private void Start() {
        this.targetRotation = this.transform.rotation.eulerAngles;
    }

    private void Update() {
        Vector3 targetRotation = new Vector3(
                this.targetX ? this.targetRotation.x : this.transform.rotation.eulerAngles.x,
                this.targetY ? this.targetRotation.y : this.transform.rotation.eulerAngles.y,
                this.targetZ ? this.targetRotation.z : this.transform.rotation.eulerAngles.z
            );

        // Does multiplying lerpSpeed by deltaTime work nicely?
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(targetRotation), this.lerpSpeed * Time.deltaTime);
    }

    public void SetTargetRotation(Quaternion rot) {
        this.targetRotation = rot.eulerAngles;
    }

    public void SetTargetRotation(Vector3 rot) {
        this.targetRotation = rot;
    }
}
