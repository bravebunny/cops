using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {
    public Transform Sensor;
    float MaxGroundDistance = 10;
    public float Speed = 20;
    Rigidbody Body;
    float CurveAngle;

    void Start() {
        Body = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        UpdateVelocity();
        if (CurveAhead()) {
            if (SimilarAngle(CurveAngle, transform.eulerAngles.y + 180, 5))
                TurnRight();
            else if (SimilarAngle(CurveAngle, transform.eulerAngles.y + 270, 5))
                TurnLeft();
        }
    }

    // look for the next train track
    bool CurveAhead() {
        RaycastHit hit;
        Vector3 origin = Sensor.position;
        Vector3 direction = -transform.up;
        bool grounded = Physics.Raycast(origin, direction, out hit, MaxGroundDistance);
        //Debug.DrawLine(origin, origin + direction * MaxGroundDistance);
        if (!grounded) return false;
        bool curveAhead = hit.transform.CompareTag("TrainCurve");
        if (curveAhead) CurveAngle = hit.transform.parent.eulerAngles.y;
        return curveAhead;
    }

    // checks if the difference between angles A and B is less than delta
    bool SimilarAngle(float a, float b, float delta) {
        Debug.Log("a: " + a + ", b: " + b + ", delta: " + delta);
        float abs = Mathf.Abs((a % 360) - (b % 360));
        return abs < delta || abs > (360 - delta);
    }

    void TurnRight() {
        Debug.Log("turn right");
        transform.eulerAngles += Vector3.up * 90;
        UpdateVelocity();
    }

    void TurnLeft() {
        Debug.Log("turn left");
        transform.eulerAngles -= Vector3.up * 90;
        UpdateVelocity();
    }

    void UpdateVelocity() {
        Body.velocity = transform.forward * Speed;
    }
}
