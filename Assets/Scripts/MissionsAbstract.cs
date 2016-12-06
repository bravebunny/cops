using UnityEngine;

public abstract class MissionsAbstract : MonoBehaviour{

    public abstract void InitiateMission();

    public abstract void EndMission();

    public abstract string GetDisplayText();
}
