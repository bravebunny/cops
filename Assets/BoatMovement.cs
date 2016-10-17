using UnityEngine;
using System.Collections;

public class BoatMovement : MonoBehaviour {

    public float degreesPerSecond = 1.0f;

    public float amplitudeY = 5.0f; //y amplitude
    public float velocityY = 1.0f; //velocity of the movement in y
    public Vector3 center = new Vector3(2, 8, 8);
    float index;

    Vector3 v;

    void Start() {
        v = transform.position - center;
    }

    public void Update() {
        v = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.up) * v;

        index += Time.deltaTime;
        float y = Mathf.Abs(amplitudeY * Mathf.Sin(velocityY * index));
        transform.position = new Vector3(center.x, y + center.y, center.z) + v;
        transform.LookAt(center, worldUp: Vector3.up);
    }
}
