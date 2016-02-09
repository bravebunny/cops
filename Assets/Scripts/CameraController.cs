using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float SmoothTime = 0.3F;
    public float Distance = 5.0F;
    public float Height = 10;
    private float yVelocity = 0.0F;
    void Update() {
        float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y + 90, ref yVelocity, SmoothTime);
        Vector3 position = target.position;
        position += Quaternion.Euler(0, yAngle, 0) * new Vector3(0, Height, -Distance);
        transform.position = position;
        transform.LookAt(target);
    }
}
