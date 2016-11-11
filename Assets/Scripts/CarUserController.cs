using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;
    public GameObject Explosion;
    public int CopCount; // number of cops nearby
    public SphereCollider CopCountArea;

    [HideInInspector] public float BustedLevel = 0;

    CarController Car;
    float Steering;
    float Accel;

    void Awake() {
        Car = GetComponent<CarController>();
    }

    void Update() {
        Steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        Accel = positive - negative;

        //if (CrossPlatformInputManager.GetButtonDown("Bomb")) Bomb();

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

        // decrease bomb counter
        GameManager.BombCount--;

        // create the explosion object
        Instantiate(Explosion, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider col) {
        CarAIController cop = col.GetComponent<CarAIController>();
        if (cop != null) CopCount++;
    }

    void OnTriggerExit(Collider col) {
        CarAIController cop = col.GetComponent<CarAIController>();
        if (cop != null) CopCount--;
    }
}
