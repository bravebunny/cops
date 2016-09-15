using UnityEngine;
using System.Collections;

public class FireHydrant : MonoBehaviour {
    public GameObject Water;
    public float MinColVelocity = 15; // minimum collision velocity that triggers destruction
    public float ActivationDelay = 0.3f; // time to wait before activating water

    Rigidbody Body;

    void Start() {
        Body = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude < MinColVelocity) return;
        
        Body.isKinematic = false;
        Body.centerOfMass = Vector3.forward;
        Body.AddForce(-collision.relativeVelocity * 0.01f + Vector3.up * 0.5f, ForceMode.Impulse);
        Body.AddTorque(Quaternion.Euler(0, 90, 0) * collision.relativeVelocity);
        StartCoroutine(ActivateWater());
    }
    
    IEnumerator ActivateWater() {
        yield return new WaitForSeconds(ActivationDelay);
        Water.SetActive(true);
    }
}
