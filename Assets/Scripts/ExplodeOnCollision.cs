using UnityEngine;
using System.Collections;

public class ExplodeOnCollision: MonoBehaviour {
    public float MinImpulse = 30; // minimum impact impulse magnitude to trigger explosion
    public GameObject Explosion;
    public bool Enabled = false;

    void OnCollisionEnter(Collision col) {
        if (!Enabled || col.impulse.magnitude < MinImpulse) return;

        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}