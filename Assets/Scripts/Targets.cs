using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
    public LookAt Arrow;
    public Transform TargetMarker;
    public float MarkerHeight = 2;

    Transform Target;
    MissionManager MissionManager;
    bool TriggerPointDisplay;

    public void SetMission(MissionManager missionManager) {
        MissionManager = missionManager;
    }

    public void SetArrowDisplay(bool state) {
        Arrow.gameObject.gameObject.SetActive(state);
        TargetMarker.gameObject.SetActive(state);
    }

    public void SetTriggerPointDisplay(bool state) {
        TriggerPointDisplay = state;
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

        ManageTriggerPointDisplay(true);
    }

    public void GoalCompleted() {
        //disable render of the mission object
        ManageTriggerPointDisplay(false);
        // remove goal behavior from previous target
        Destroy(Target.GetComponent<Goal>());
        MissionManager.GoalCompleted();
    }

    public void MissionFailed() {
        //disable render of the mission object
        ManageTriggerPointDisplay(false);
        // remove goal behavior from previous target
        Destroy(Target.GetComponent<Goal>());
    }

    void ManageTriggerPointDisplay(bool state) {
        if (TriggerPointDisplay) {
            Target.gameObject.GetComponent<MeshRenderer>().enabled = state;
        }
    }
}
