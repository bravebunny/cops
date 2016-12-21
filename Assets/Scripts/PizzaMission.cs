using UnityEngine;
using System.Collections;
using System;

public class PizzaMission : MissionsAbstract {

    public GameObject Target;
    MissionManager MM;

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pizza Mission");
        MM = missionManager;
        MM.TargetScript.SetMission(this, true, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(Target);
    }

    public override void EndMission() {
        Debug.Log("End Pizza Mission");
        //CollectSound.Play();
        MM.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pizza";
    }
}
