using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;
    public float ExplostionRadius = 10;
    public float ExplosionPower = 10;
    public GameObject Explosion;
    [HideInInspector] public float BustedLevel = 0;

    CarController Car;
    Rigidbody Body;
    float Steering;
    float Accel;

    void Awake() {
        Car = GetComponent<CarController>();
        Body = GetComponent<Rigidbody>();
    }

    void Update() {
        Steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        Accel = positive - negative;

        if (CrossPlatformInputManager.GetButtonDown("Bomb")) Bomb();

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

    void Bomb() {
        if (GameManager.BombCount <= 0) return;
        GameManager.BombCount--;
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplostionRadius);
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(ExplosionPower, transform.position, ExplostionRadius, 0F);
            }
        }
    }
}
