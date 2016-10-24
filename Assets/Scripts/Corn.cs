using UnityEngine;
using System.Collections;

public class Corn : MonoBehaviour {
    bool Fall;
    Vector3 Direction;
    Rigidbody Body;
    BoxCollider Collider;
    public float Speed = 0.01F;
    float Randomness = 0.1f;
	
    void Start() {
        Body = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        Randomize();
    }

    void Randomize() {
        float x = Random.Range(-Randomness, Randomness);
        float z = Random.Range(-Randomness, Randomness);
        Vector3 random = new Vector3(x, 0, z);
        transform.up += random;
        transform.position += random;
    }

	void FixedUpdate () {
        if (!Fall) return;
        Vector3 newDir = Vector3.RotateTowards(transform.up, Direction, Speed, 0.0F);
        transform.up = newDir;
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.GetComponent<Corn>() != null) return;
        Collider.enabled = false;
        Fall = true;
        Direction = (transform.position - coll.transform.position).normalized;
    }
}
