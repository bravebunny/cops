using System.Collections.Generic;
using UnityEngine;

public abstract class MissionsAbstract : MonoBehaviour{

    public abstract void InitiateMission(MissionManager missionManager, int targetIndex = 0);

    public abstract void EndMission();

    public abstract string GetDisplayText();
}
