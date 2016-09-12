using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    Rigidbody CarBody;
    public GameObject Particles;
    public float Radius = 50;
    public float Power = 2000;
    public float VerticalModifier = 5;
    public CameraShake Camera;

    void Start () {
        CarBody = GetComponent<Rigidbody>();
    }

    public void Explode() {
        if (GameManager.BombCount <= 0) return;

        // shake the camera
        Camera.Shake();

        // decrease bomb counter
        GameManager.BombCount--;

        // spawn explosion particles
        Instantiate(Particles, transform.position, Quaternion.identity);

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
