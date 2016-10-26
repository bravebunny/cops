using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {
    public Transform Sensor;
    float MaxGroundDistance = 10;
    public float Speed = 20;
    public float RotationSpeed = 5;
    Vector3 Target;

    void Start() {
    }

    void FixedUpdate() {
        UpdateTarget();
        Align();
        MoveTowardsTarget();
    }

    void Align() {
        Vector3 forward = (Target - transform.position).normalized;
        forward.y = transform.forward.y;
        transform.forward = Vector3.RotateTowards(transform.forward, forward, RotationSpeed, 0);
    }

    void MoveTowardsTarget() {
        transform.position += transform.forward * Speed;
    }

    void UpdateTarget() {
        Track backTrack = GetFrontTrack();
        if (backTrack == null) return;

        Vector3 a = backTrack.A.position;
        Vector3 b = backTrack.B.position;

        // check which point is in front
        Vector3 heading = a - Sensor.position;
        float dot = Vector3.Dot(heading, transform.forward);
        if (dot > 0) Target = a;
        else Target = b; // if A isn't in front, B must be
    }

    // get the track currently under the back of the train
    Track GetBackTrack() {
        return GetTrack(transform.position);
    }

    // get the track currently under the front of the train
    Track GetFrontTrack() {
        return GetTrack(Sensor.position);
    }

    // get the track under given origin
    Track GetTrack(Vector3 origin) {
        RaycastHit hit;
        Vector3 direction = -transform.up;
        bool grounded = Physics.Raycast(origin, direction, out hit, MaxGroundDistance);
        Debug.DrawLine(origin, origin + direction * MaxGroundDistance);
        if (!grounded) return null;
        return hit.transform.GetComponent<Track>();
    }
}
