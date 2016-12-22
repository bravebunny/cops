using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PizzaMission : MissionsAbstract {

    public List<MissionTargets> MissionTargets;
    public GameObject MissionModel;
    MissionManager MM;

    int TargetIndex = 0;

    public override void InitiateMission(MissionManager missionManager, int targetIndex = 0) {
        if (MissionModel) {
            GameManager.Player.ReplaceModel(MissionModel);
        }

        TargetIndex = targetIndex;
        Debug.Log("Initiate " + MissionTargets[targetIndex].MissionDescription);
        MM = missionManager;
        MM.TargetScript.SetMission(this, true, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(MissionTargets[targetIndex].Target);
    }

    public override void EndMission() {
        Debug.Log("End Pizza Mission");
        //CollectSound.Play();
        if (MissionTargets.Count - (TargetIndex + 1) == 0)
            MM.EndCurrentMission(false);
        else
            MM.EndCurrentMission(true, TargetIndex);
    }

    public override string GetDisplayText() {
        Debug.Log("TargetIndex: " + TargetIndex);
        return MissionTargets[TargetIndex].MissionDescription;
    }
}
