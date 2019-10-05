using UnityEngine;
using System.Collections;

public class BallCamera : MonoBehaviour {

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zoomSpeed = 5.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public bool mousePressMovement = false;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start() {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null) {
            rigidbody.freezeRotation = true;
        }
    }


    // Late Update
    void LateUpdate() {
        if (target) {

            //Kameran liikuttaminen hiirellä
            if (mousePressMovement == false || Input.GetMouseButton(1)) {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);


            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;


            // Zoomaus hiiren rullalla
            if (Input.GetAxis("Mouse ScrollWheel") != 0) {

                float distance_wallceck_dist = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, distanceMin, distanceMax);
                Vector3 distance_wallcheck_vector3 = new Vector3(0.0f, 0.0f, -distance_wallceck_dist);

                // Tarkistaa että on tilaa rullata kauemmas tai eteen
                if (!Physics.Linecast(position, rotation * distance_wallcheck_vector3 + position)) {
                    distance = distance_wallceck_dist;
                }
            }

            //Jos seinä tulee vastaan kameralle
            int yritys = 0;
            float start_y = y;

            Vector3 start_position = position;

            float radius_bonus = transform.parent.GetComponent<BallMovement>().sphere_radius * 1.5f;
            Vector3 linecast_start = new Vector3(target.position.x, target.position.y + radius_bonus, target.position.z);
            Vector3 linecast_end = new Vector3(position.x, position.y + 0.5f, position.z);

            while (Physics.Linecast(linecast_start, linecast_end)) {

                switch (yritys) {

                    case 0:
                        if (rotation.eulerAngles.x > yMinLimit) {
                            y -= 0.1f;
                            rotation = Quaternion.Euler(y, x, 0);
                        }
                        else { yritys = 1; }
                        break;


                    case 1:
                        if (rotation.eulerAngles.x < yMaxLimit) {
                            y += 0.1f;
                            rotation = Quaternion.Euler(y, x, 0);
                        }
                        else { yritys = 2; }
                        break;


                    case 2:
                        if (y != start_y) {
                            y = start_y;
                            rotation = Quaternion.Euler(y, x, 0);
                        }
                        distance -= 1;
                        break;

                }

                if (!Physics.Linecast(linecast_start, transform.position)) { break; }
                position = rotation * negDistance + target.position;
                transform.position = position;
                transform.rotation = rotation;

                if (distance < distanceMin) { break; }

            }

            //Rotationin ja positionin asettaminen
            transform.position = position;
            transform.rotation = rotation;

            //Debug.DrawLine(linecast_start, linecast_end);
        }
    }


    // Clamp Angle
    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}