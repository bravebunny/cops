using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;

    [HideInInspector] public float BustedLevel = 0;

    CarController Car;
    Explosion Explosion;
    float Steering;
    float Accel;

    void Awake() {
        Car = GetComponent<CarController>();
        Explosion = GetComponent<Explosion>();
    }

    void Update() {
        Steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        Accel = positive - negative;

        if (CrossPlatformInputManager.GetButtonDown("Bomb")) Explosion.Explode();

        if (BustedLevel > 0) BustedLevel -= BustedDecRate;

        // temporary way to drown player
        if (transform.position.y < -10) BustedLevel = int.MaxValue;
    }

    void FixedUpdate() {
        Car.Move(Steering, Accel);
    }

    void OnCollisionStay(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Cop":
                BustedLevel += BustedIncRate;
                break;
        }

    }


}
