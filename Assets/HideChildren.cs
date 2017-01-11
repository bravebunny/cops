using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideChildren : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "MainCamera") {
            foreach (Transform child in transform) {
                if (child.GetComponent<Renderer>())
                    child.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "MainCamera") {
            foreach (Transform child in transform) {
                if (child.GetComponent<Renderer>())
                    child.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
