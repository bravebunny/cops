using System;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    public GameObject Cars;
    MissionManager MM;

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pursuit Car Mission");

        MM = missionManager;
        Cars.GetComponent<Targets>().SetMission(this, true);
        Cars.GetComponent<Targets>().NewTarget();
    }

    public override void EndMission() {
        Debug.Log("End Pursuit Car Mission");
        MM.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pursuit car";
    }
}
