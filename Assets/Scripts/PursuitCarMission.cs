using System.Collections.Generic;
using UnityEngine;

public class PursuitCarMission :  MissionsAbstract {

    MissionManager MM;

    public List<MissionTargets> MissionTargets;
    public GameObject MissionModel;
    public GameObject Cargo;
    //public AudioSource CollectSound; //Sound that plays when mission's objective is completed 
    public int NumberOfFases;
    public int CargoFase;
    public bool ShowArrow;
    public bool ShowTriggerPoint;

    public override void InitiateMission(MissionManager missionManager) {
        MM = missionManager;

        MM.SetArrowDisplay(ShowArrow);
        MM.SetTriggerPointDisplay(ShowTriggerPoint);
        MM.SetCargo(Cargo, CargoFase);

        MM.InitMission(this, MissionTargets, MissionModel, NumberOfFases);
    }
}
