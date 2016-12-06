using UnityEngine;
using System.Collections;
using System;

public class PizzaMission : MissionsAbstract {

    MissionManager MM;

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pizza Mission press space to skip");
        MM = missionManager;
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
