using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PizzaMission : MissionsAbstract {

    public List<MissionTargets> MissionTargets;
    public GameObject MissionModel;
    public GameObject Cargo;
    MissionManager MM;

    int TargetIndex = 0;

    public override void InitiateMission(MissionManager missionManager, int targetIndex = 0) {

        if (MissionModel) {
            GameManager.Player.ReplaceModel(MissionModel);
        }

        TargetIndex = targetIndex;
        MM = missionManager;
        MM.TargetScript.SetMission(this, true, true);
        MM.TargetScript.GetComponent<Targets>().NewTarget(MissionTargets[targetIndex].Target);
    }

    public override void MissionCompleted() {
        //CollectSound.Play();
        if (MissionTargets.Count - (TargetIndex + 1) == 0) {
            MM.MissionCompleted(false);
        } else {
            MM.MissionCompleted(true, TargetIndex);
            if (Cargo) {
                GameManager.Player.Model.GetComponent<SpawnCargo>().Spawn(Cargo, this);
            }
        }
    }

    public override void MissionFailed() {
        MM.MissionFailed();
    }

    public override string GetDisplayText() {
        return MissionTargets[TargetIndex].MissionDescription;
    }
}
