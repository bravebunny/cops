using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
    public LookAt Arrow;
    public Transform TargetMarker;
    public float MarkerHeight = 2;
    Transform Target;

	void Start() {
        NewTarget();
	}

    public void NewTarget() {
        int index = Random.Range(0, transform.childCount);
        Target = transform.GetChild(index);
        TargetMarker.position = Target.position + Vector3.up * MarkerHeight;
        TargetMarker.SetParent(Target);
        Arrow.Target = Target;
    }
}
