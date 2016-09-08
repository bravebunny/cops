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
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        Car.Move(h, v);

        if (BustedLevel > 0) BustedLevel -= BustedDecRate;
    }

    void OnCollisionStay(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Cop":
                BustedLevel += BustedIncRate;
                break;
        }

    }
}
