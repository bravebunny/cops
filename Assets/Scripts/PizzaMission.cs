using UnityEngine;
using System.Collections;
using System;

public class PizzaMission : MissionsAbstract {

    public GameObject Target;
    MissionManager MM;

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pizza Mission press space to skip");
        MM = missionManager;
        MM.TargetScript.SetMission(this, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(Target);
    }

    public override void EndMission() {
        Debug.Log("End Pizza Mission");
    }

    void Update() {
        if (Input.GetKeyDown("space"))
            MM.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pizza";
    }
}
