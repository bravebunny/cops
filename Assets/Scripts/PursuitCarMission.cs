using System;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    public GameObject Target;
    MissionManager MM;

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pursuit Car Mission");

        MM = missionManager;
        MM.TargetScript.SetMission(this, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(Target);
    }

    public override void EndMission() {
        Debug.Log("End Pursuit Car Mission");
        MM.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pursuit car";
    }
}
