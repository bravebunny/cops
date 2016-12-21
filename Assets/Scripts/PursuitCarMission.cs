using System;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    public GameObject Target; //Mission target
    MissionManager MM; //Mission Manager
    AudioSource CollectSound; //Sound that plays when mission's objective is completed 

    public override void InitiateMission(MissionManager missionManager) {
        Debug.Log("Initiate Pursuit Car Mission");

        MM = missionManager;
        MM.TargetScript.SetMission(this, true); //Set mission on Targets script 
        MM.TargetScript.GetComponent<Targets>().NewTarget(Target); //Set target in Targets script
        CollectSound = Target.GetComponent<AudioSource>();
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
