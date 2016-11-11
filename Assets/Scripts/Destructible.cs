using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        if (collider.isTrigger) return;
        foreach (Transform child in transform) {
            //enable physics
            child.GetComponent<Rigidbody>().isKinematic = false;

            //enable collisions if usnig box collider
            if (child.GetComponent<BoxCollider>())
                child.GetComponent<BoxCollider>().enabled = true;
            if (child.GetComponent<MeshCollider>())
                child.GetComponent<MeshCollider>().enabled = true;
        }
    }
}
