using UnityEngine;
using System.Collections.Generic;

public class PizzaMission : MissionsAbstract {
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

        MM.InitMission(this, MissionTargets, MissionModel, NumberOfFases);
        MM.SetCargo(Cargo, CargoFase);

        MM.SetArrowDisplay(ShowArrow);
        MM.SetTriggerPointDisplay(ShowTriggerPoint);
    }
}
