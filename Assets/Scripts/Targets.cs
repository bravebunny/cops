using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
    public LookAt Arrow;
    public Transform TargetMarker;
    public float MarkerHeight = 2;
    Transform Target;
    MissionsAbstract CurrentMission;
    bool ArrowDisplay = false; //show Arrow on Screen 

    public void SetMission(MissionsAbstract currentMission, bool arrowDisplay) {
        CurrentMission = currentMission;
        ArrowDisplay = arrowDisplay;

        //show Arrow on Screen 
        if (ArrowDisplay) {
            Debug.Log("show target");
            TargetMarker.gameObject.SetActive(true);
            Arrow.gameObject.gameObject.SetActive(true);
        } else {
            TargetMarker.gameObject.SetActive(false);
            Arrow.gameObject.gameObject.SetActive(false);
        }
    }

    public void NewTarget(GameObject target) {

        // pick new target randomly from children
        int index = Random.Range(0, target.transform.childCount);
        Target = target.transform.GetChild(index);

        // add goal behavior to new target
        Goal goal = Target.gameObject.AddComponent<Goal>();
        goal.TargetManager = this;

        // add marker that follows target around
        TargetMarker.position = Target.position + Vector3.up * MarkerHeight;
        TargetMarker.SetParent(Target);

        // make the arrow point to the new target
        Arrow.Target = Target;

    }

    public void DestroyTarget() {
        // remove goal behavior from previous target
        Destroy(Target.GetComponent<Goal>());
        CurrentMission.EndMission();
    }
}
