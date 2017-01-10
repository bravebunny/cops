using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour {
    public GameObject SplashPrefab;
    public float MinVelocity = 30;
    public float ImpulseForce = 1.5f;
    public float Resistance = 4;

    void OnTriggerEnter(Collider col) {
        InstantiateSplash(col);
        SkipOnWater(col);
    }

    void OnTriggerExit(Collider col) {
        InstantiateSplash(col);
    }

    void SkipOnWater(Collider col) {
        Rigidbody body = col.GetComponentInParent<Rigidbody>();
        if (body == null) return;

        Vector3 velocity = new Vector3(body.velocity.x, 0, body.velocity.z) / 2;
        if (velocity.magnitude > MinVelocity) {
            Vector3 force = (Vector3.down * ImpulseForce * body.velocity.y) - (velocity.normalized * Resistance);
            Vector3 forcePosition = transform.position - velocity.normalized;
            body.AddTorque(transform.right * ImpulseForce * 1000);
            body.AddForceAtPosition(force, forcePosition, ForceMode.Impulse);
        }
    }

    void InstantiateSplash(Collider col) {
        GameObject splash = (GameObject)Instantiate(SplashPrefab);
        splash.transform.position = col.transform.position;
    }
}
