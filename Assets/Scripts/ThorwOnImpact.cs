using UnityEngine;
using System.Collections;

public class ThorwOnImpact : MonoBehaviour {
    public float ImpulseStrength = 10;
    float UpMultiplier = 1;
    float ForwardMultiplier = 0.75f;

	void OnCollisionEnter(Collision col) {
        Debug.Log("cmon");
        Rigidbody body = col.transform.GetComponent<Rigidbody>();
        if (body == null) return;
        Vector3 up = Vector3.up * UpMultiplier;
        Vector3 forward = ((transform.position - col.transform.position) + transform.parent.forward).normalized * ForwardMultiplier;
        Vector3 force = (up + forward) * ImpulseStrength;
        body.AddForce(force, ForceMode.Impulse);
    }

    void Update() {
        Debug.DrawLine(transform.position, transform.position + transform.parent.forward * 10, Color.red, 0, false);
    }
}
