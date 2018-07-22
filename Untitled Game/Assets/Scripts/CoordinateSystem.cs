using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CoordinateSystem {
    public Vector3 X { get; private set; }
    public Vector3 Y { get; private set; }
    public Vector3 Z { get; private set; }
    public Vector3 O { get; private set; }

    public CoordinateSystem(Vector3 x, Vector3 y, Vector3 z, Vector3 o = new Vector3()) {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.O = o;
    }

    public Vector3 ToDefault(Vector3 vec) {
        Vector3 x = Vector3.Project(vec, this.X);
        Vector3 y = Vector3.Project(vec, this.Y);
        Vector3 z = Vector3.Project(vec, this.Z);

        return x + y + z + this.O;
    }

    public Vector3 FromDefault(Vector3 vec) {
        Vector3 vecInDefaultMinusO = vec - this.O;

        float toX = Vector3.Project(vecInDefaultMinusO, this.X).magnitude;
        float toY = Vector3.Project(vecInDefaultMinusO, this.Y).magnitude;
        float toZ = Vector3.Project(vecInDefaultMinusO, this.Z).magnitude;

        return new Vector3(toX, toY, toZ);
    }

    public static Vector3 Convert(Vector3 vec, CoordinateSystem from, CoordinateSystem to) {
        Vector3 vecInDefault = from.ToDefault(vec);
        return to.FromDefault(vecInDefault);
    }

    public static CoordinateSystem Default {
        get {
            return new CoordinateSystem(Vector3.right, Vector3.up, Vector3.forward);
        }
    }
}
