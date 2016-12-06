using UnityEngine;
using System.Collections;

public class DestructibleSimple : MonoBehaviour {

    void OnCollisionEnter(Collision collision) {
        transform.GetComponent<Rigidbody>().isKinematic = false;
        GetComponentInChildren<OpenGate>().enabled = false;
    }
}