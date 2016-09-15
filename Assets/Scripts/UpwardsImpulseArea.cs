using UnityEngine;
using System.Collections;

public class UpwardsImpulseArea : MonoBehaviour {
    public float ImpulseStrenght = 100;

    void OnTriggerStay (Collider col) {
        Rigidbody body = col.GetComponent<Rigidbody>();
        body.AddForceAtPosition(Vector3.up * ImpulseStrenght, transform.position);
    }
}
