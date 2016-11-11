using UnityEngine;
using System.Collections;

public class ExplodeOnImpact : MonoBehaviour {
    public float MinImpulse = 30; // minimum impact impulse magnitude to trigger explosion
    public float MinHeight = 10; // minimum hight to enable explosion
    public GameObject Explosion;
    bool Enabled = false;
    public float CheckRate = 0.5f;

    void Start() {
        InvokeRepeating("CheckHeight", 0, CheckRate);
    }

    void CheckHeight() {
        bool grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, MinHeight);
        if (!grounded) Enabled = true;
    }

	void OnCollisionEnter(Collision col) {
        if (!Enabled) return;
        if (col.impulse.magnitude < MinImpulse) {
            Enabled = false;
            return;
        }
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
