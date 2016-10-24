using UnityEngine;
using System.Collections;

public class Corn : MonoBehaviour {
    bool Fall;
    Vector3 Direction;
    Rigidbody Body;
    BoxCollider Collider;
    public float Speed = 0.01F;
    float AngleRandomness = 0.1f;
	
    void Start() {
        Body = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        RandomizeAngle();
    }

    void RandomizeAngle() {
        float x = Random.Range(-AngleRandomness, AngleRandomness);
        float z = Random.Range(-AngleRandomness, AngleRandomness);
        transform.up = new Vector3(x, 1, z);
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
