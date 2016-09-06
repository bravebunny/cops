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

        // remove goal behavior from previous target
        if (Target) {
            Target.gameObject.tag = "Untagged";
            Destroy(Target.GetComponent<Goal>());
        }

        // pick new target randomly from children
        int index = Random.Range(0, transform.childCount);
        Target = transform.GetChild(index);
        Target.gameObject.tag = "Target";

        // add goal behavior to new target
        Goal goal = Target.gameObject.AddComponent<Goal>();
        goal.TargetManager = this;

        // add marker that follows target around
        TargetMarker.position = Target.position + Vector3.up * MarkerHeight;
        TargetMarker.SetParent(Target);

        // make the arrow point to the new target
        Arrow.Target = Target;
    }
}
