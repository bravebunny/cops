using UnityEngine;

public abstract class MissionsAbstract : MonoBehaviour{

    public abstract void InitiateMission(MissionManager missionManager);

    public abstract void EndMission();

    public abstract string GetDisplayText();
}
