using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfOutsideTrigger : MonoBehaviour {
    public string Tag = "Road";

    void Start () {
        bool destroy = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider col in colliders) {
            if (col.tag == Tag) destroy = false;
        }
        if (destroy) Destroy(gameObject);
    }
}
