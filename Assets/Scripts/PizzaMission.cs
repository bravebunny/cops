using UnityEngine;
using System.Collections;
using System;

public class PizzaMission : MissionsAbstract {
    public override void InitiateMission() {
        Debug.Log("Initiate Pizza Mission press space to skip");
    }

    public override void EndMission() {
        Debug.Log("End Pizza Mission");
    }

    void Update() {
        if (Input.GetKeyDown("space"))
            MissionManager.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pizza";
    }
}
