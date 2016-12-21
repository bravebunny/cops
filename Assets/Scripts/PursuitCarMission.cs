using System;
using System.Collections.Generic;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    public List<GameObject> Targets; //Mission target
    MissionManager MM; //Mission Manager
    AudioSource CollectSound; //Sound that plays when mission's objective is completed 

    public override void InitiateMission(MissionManager missionManager, int targetIndex = 0) {
        Debug.Log("Initiate Pursuit Car Mission");

        MM = missionManager;
        MM.TargetScript.SetMission(this); //Set mission on Targets script 
        MM.TargetScript.GetComponent<Targets>().NewTarget(Targets[targetIndex]); //Set target in Targets script
        CollectSound = Targets[targetIndex].GetComponent<AudioSource>();
    }

    public override void EndMission() {
        Debug.Log("End Pursuit Car Mission");
        CollectSound.Play();
        MM.EndCurrentMission();
    }

    public override string GetDisplayText() {
        return "pursuit car";
    }
}
