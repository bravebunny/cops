using UnityEngine;
using System.Collections;

public class UpwardsImpulseArea : MonoBehaviour {
    public float ImpulseStrenght = 100;

    void OnTriggerStay (Collider col) {
        if (col.isTrigger) return;
        Rigidbody body = col.GetComponentInParent<Rigidbody>();
        body.AddForceAtPosition(Vector3.up * ImpulseStrenght, transform.position);
    }
}
