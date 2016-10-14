using UnityEngine;
using System.Collections;

public class BalloonMovement : MonoBehaviour {

    Transform center;
    public float degreesPerSecond = -65.0f;

    private Vector3 v;

    void Start() {
        v = transform.position - new Vector3(2,8,8);
    }

    void Update() {
        v = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.up) * v;
        transform.position = new Vector3(2, 8, 8) + v;
    }
}
