using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    CarController Car;
    Rigidbody Body;
    [HideInInspector] public float BustedLevel = 0;
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;
    public float ExplostionRadius = 10;
    public float ExplosionPower = 10;

    void Awake() {
        Car = GetComponent<CarController>();
        Body = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        float steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        float accel = positive - negative;

        Car.Move(steering, accel);

        if (CrossPlatformInputManager.GetButtonDown("Bomb")) Bomb();

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

    void Bomb() {
        if (GameManager.BombCount <= 0) return;
        GameManager.BombCount--;

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplostionRadius);
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(ExplosionPower, transform.position, ExplostionRadius, 0F);
            }

        }
    }
}
