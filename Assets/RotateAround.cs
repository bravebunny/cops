using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
    public Transform Pivot;
    public float Speed = 10;
    Quaternion InitialRotation;

    void Start() {
        InitialRotation = transform.localRotation;
    }
	
	void FixedUpdate() {
        transform.RotateAround(Pivot.position, transform.parent.forward, Speed);
        transform.localRotation = InitialRotation;
    }
}
