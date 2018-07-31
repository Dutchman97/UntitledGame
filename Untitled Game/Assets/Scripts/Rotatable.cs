using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rotatable : MonoBehaviour {
    [Tooltip("The rotation linear interpolation speed")]
    public float rotationLerpSpeed;

    protected Quaternion rotationTarget;
    private Quaternion rotationOffset;

    private void Start() {
        this.rotationTarget = this.rotationOffset = this.TransformToRotate.rotation;
    }

    private void FixedUpdate() {
        Transform transformToRotate = this.TransformToRotate;
        transformToRotate.rotation = Quaternion.Slerp(transformToRotate.rotation, this.rotationTarget, this.rotationLerpSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Make the component's rotation target the given rotation.
    /// </summary>
    /// <param name="rotation">The rotation the component should target.</param>
    public void TargetRotation(Quaternion rotation) {
        this.rotationTarget = rotation * this.rotationOffset;
    }

    /// <summary>
    /// The transform that should be rotated should be returned by this function.
    /// </summary>
    protected abstract Transform TransformToRotate { get; }
}
