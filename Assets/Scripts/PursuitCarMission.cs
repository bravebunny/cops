using System;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    public GameObject Cars;

    public override void InitiateMission() {
        Debug.Log("Initiate Pursuit Car Mission");
        Cars.GetComponent<Targets>().NewTarget();
    }

    public override void EndMission() {
        Debug.Log("End Pursuit Car Mission");
        Cars.GetComponent<Targets>().DestroyTarget();
    }

    public override string GetDisplayText() {
        return "pursuit car";
    }
}
