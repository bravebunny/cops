using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    Rigidbody CarBody;
    public float Radius = 50;
    public float Power = 2000;
    public float VerticalModifier = 5;

    void Start () {
        CarBody = GameManager.Player.GetComponent<Rigidbody>();
        Explode();
    }

    public void Explode() {
        // shake the camera
        GameManager.GameCamera.GetComponent<CameraShake>().Shake();

        // apply explosion force to surrounding objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb != CarBody) {
                rb.AddExplosionForce(Power, transform.position, Radius, VerticalModifier);
            }
        }
    }
}
