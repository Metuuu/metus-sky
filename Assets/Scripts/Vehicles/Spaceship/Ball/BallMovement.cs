using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

    public float movement_speed;
    public float jump_force;
    public float max_movement_speed;
    public float max_fall_speed;

    public float max_angular_velocity;

    public float sphere_radius;

    private Rigidbody character;
    private SphereCollider spherecollider;

    private Vector3 CameraVerticalRotation;
    private Vector3 CameraHorizontalRotation;

    //START
    void Start() {
        character = GetComponent<Rigidbody>();
    }

    //FIXED UPDATE
    void FixedUpdate() {

        // -- LIIKKUMINEN --
        CameraVerticalRotation = Camera.main.transform.forward;
        CameraHorizontalRotation = Camera.main.transform.right;

        CameraHorizontalRotation.y = 0;
        CameraVerticalRotation.y = 0;

        //if (IsGrounded()) {
        if (Input.GetKey(KeyCode.W)) character.AddTorque(CameraHorizontalRotation * movement_speed, ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.A)) character.AddTorque(CameraVerticalRotation * movement_speed, ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.S)) character.AddTorque(CameraHorizontalRotation * -1 * movement_speed, ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.D)) character.AddTorque(CameraVerticalRotation * -1 * movement_speed, ForceMode.Acceleration);
        //}

        // -- HYPPY --
        if (Input.GetKey(KeyCode.Space) && IsGrounded()) {
            character.AddForce(Vector3.up * jump_force, ForceMode.Impulse);
        }


        // -- MAX SPEEDS --

        Vector3 vector = character.velocity;

        // Max rotation speed
        character.maxAngularVelocity = max_angular_velocity;


        // Max movement speed

        // X
        if (character.velocity.x > 0) {
            if (character.velocity.x > max_movement_speed) {
                vector.x = max_movement_speed;
            }
            else { vector.x = character.velocity.x; }
        }
        else {
            if (character.velocity.x < max_movement_speed * -1) {
                vector.x = max_movement_speed * -1;
            }
            else { vector.x = character.velocity.x; }
        }

        // Z
        if (character.velocity.z > 0) {
            if (character.velocity.z > max_movement_speed) {
                vector.z = max_movement_speed;
            }
            else { vector.z = character.velocity.z; }
        }
        else {
            if (character.velocity.z < max_movement_speed * -1) {
                vector.z = max_movement_speed * -1;
            }
            else { vector.z = character.velocity.z; }
        }


        // Max fall speed
        if (character.velocity.y < 0) {
            if (character.velocity.y < max_fall_speed * -1) {
                vector.y = max_fall_speed * -1;
            }
            else { vector.y = character.velocity.y; }
        }



        character.velocity = vector;


    }

    //IS GROUNDED
    public float GroundDistance;
    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, GroundDistance + 0.1f);
    }

}