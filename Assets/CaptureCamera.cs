using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCamera : MonoBehaviour {
    public Vector2 Speed;
    public float RotationSpeed;
    public float SlowMoRate = 0.5f;

    float InitialDeltaTime;

    void Start() {
        InitialDeltaTime = Time.fixedDeltaTime;
    }

	void FixedUpdate() {
        // camera movement
        if (Input.GetKey(KeyCode.Keypad8)) Move(0, 1);  // up
        if (Input.GetKey(KeyCode.Keypad2)) Move(0, -1); // down
        if (Input.GetKey(KeyCode.Keypad4)) Move(-1, 0); // left
        if (Input.GetKey(KeyCode.Keypad6)) Move(1, 0);  // right

        SlowMoOff();
        if (Input.GetMouseButton(0)) SlowMoOn();
    }

    void Move(int x, int y) {
        transform.position += transform.rotation * new Vector3(Speed.x * x, 0, Speed.y * y);
    }

    void SlowMoOn() {
        Time.fixedDeltaTime = InitialDeltaTime * SlowMoRate;
        Time.timeScale = SlowMoRate;
    }

    void SlowMoOff() {
        Time.fixedDeltaTime = InitialDeltaTime;
        Time.timeScale = 1;
    }
}
