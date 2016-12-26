using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
    public LookAt Arrow;
    public Transform TargetMarker;
    public float MarkerHeight = 2;

    Transform Target;
    MissionsAbstract CurrentMission;
    //show Arrow on Screen 
    bool ArrowDisplay = false;
    //requires disabling render of mission's object after the mission is completed
    bool RequireDisabledRender = false; 

    public void SetMission(MissionsAbstract currentMission, bool arrowDisplay = true, bool requireDisabledRender = false) {
        CurrentMission = currentMission;
        ArrowDisplay = arrowDisplay;
        RequireDisabledRender = requireDisabledRender;

        //show Arrow on Screen 
        if (ArrowDisplay) {
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

        if (RequireDisabledRender) {
            Target.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

    }

    public void DestroyTarget() {
        //disable render of the mission object
        if (RequireDisabledRender) {
            Target.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        // remove goal behavior from previous target
        Destroy(Target.GetComponent<Goal>());
        CurrentMission.EndMission();
    }
}
