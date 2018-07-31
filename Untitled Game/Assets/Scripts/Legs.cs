using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : Rotatable {
    [Tooltip("The (bone) transform that should be rotated.")]
    public Transform boneTransform;

    protected override Transform TransformToRotate {
        get {
            return this.boneTransform;
        }
    }
}
