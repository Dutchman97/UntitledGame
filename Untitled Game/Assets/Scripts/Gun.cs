using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Rotatable {
    [Tooltip("The (bone) transform that should be rotated.")]
    public Transform boneTransform;

    /// <summary>
    /// Make the gun look at the specified point in world-space.
    /// </summary>
    /// <param name="point">The position of the point in world-space.</param>
    public void LookAt(Vector3 point) {
        Vector3 lookDirection = (point - this.boneTransform.position).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, lookDirection);
        Vector3 up = Vector3.Cross(lookDirection, right);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, up);
        this.TargetRotation(lookRotation);
    }

    protected override Transform TransformToRotate {
        get {
            return this.boneTransform;
        }
    }
}
