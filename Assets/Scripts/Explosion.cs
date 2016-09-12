using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    Rigidbody CarBody;
    public GameObject Original;
    public float Radius = 50;
    public float Power = 2000;
    public float VerticalModifier = 5;

    void Start () {
        CarBody = GetComponent<Rigidbody>();
    }

    public void Explode() {
        if (GameManager.BombCount <= 0) return;
        GameManager.BombCount--;
        Instantiate(Original, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb != CarBody) {
                rb.AddExplosionForce(Power, transform.position, Radius, VerticalModifier);
            }
        }
    }
}
