using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody Data
public class RigidbodyData {

    static float mass;
    static float drag;
    static float angularDrag;
    static bool useGravity;
    static bool isKinematic;
    static RigidbodyInterpolation interpolation;
    static CollisionDetectionMode collisionDetection;
    static RigidbodyConstraints constraints;

    public void saveRigidbody(Rigidbody rb) {
        mass = rb.mass;
        drag = rb.drag;
        angularDrag = rb.angularDrag;
        useGravity = rb.useGravity;
        isKinematic = rb.isKinematic;
        interpolation = rb.interpolation;
        collisionDetection = rb.collisionDetectionMode;
        constraints = rb.constraints;
    }

    public void restoreRigidbody(Rigidbody rb) {
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
        rb.useGravity = useGravity;
        rb.isKinematic = isKinematic;
        rb.interpolation = interpolation;
        rb.collisionDetectionMode = collisionDetection;
        rb.constraints = constraints;
    }

}