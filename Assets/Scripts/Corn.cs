using UnityEngine;
using System.Collections;

public class Corn : MonoBehaviour {
    bool Fall;
    Vector3 Direction;
    Rigidbody Body;
    BoxCollider Collider;
    public float Speed = 0.01F;
	
    void Start() {
        Body = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
    }

	void FixedUpdate () {
        if (!Fall) return;
        Vector3 newDir = Vector3.RotateTowards(transform.up, Direction, Speed, 0.0F);
        transform.up = newDir;
    }

    void OnTriggerEnter(Collider coll) {
        Collider.enabled = false;
        //Body.isKinematic = false;
        Fall = true;
        Direction = (transform.position - coll.transform.position).normalized;
    }
}
