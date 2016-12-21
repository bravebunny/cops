using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PizzaMission : MissionsAbstract {

    public List<GameObject> Targets;
    MissionManager MM;

    int TargetIndex = 0;

    public override void InitiateMission(MissionManager missionManager, int targetIndex = 0) {
        TargetIndex = targetIndex;
        Debug.Log("Initiate Pizza Mission");
        MM = missionManager;
        MM.TargetScript.SetMission(this, true, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(Targets[targetIndex]);
    }

    public override void EndMission() {
        Debug.Log("End Pizza Mission");
        //CollectSound.Play();
        if (Targets.Count - (TargetIndex + 1) == 0)
            MM.EndCurrentMission(false);
        else
            MM.EndCurrentMission(true, TargetIndex);
    }

    public override string GetDisplayText() {
        return "pizza";
    }
}
