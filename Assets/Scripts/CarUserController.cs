using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    CarController Car;
    [HideInInspector] public float BustedLevel = 0;
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;

    void Awake() {
        Car = GetComponent<CarController>();
    }

    void FixedUpdate () {
        float steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        float accel = positive - negative;

        Car.Move(steering, accel);

        if (BustedLevel > 0) BustedLevel -= BustedDecRate;

        // temporary way to drown player
        if (transform.position.y < -10) BustedLevel = int.MaxValue;
    }

    void OnCollisionStay(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Cop":
                BustedLevel += BustedIncRate;
                break;
        }

    }
}
