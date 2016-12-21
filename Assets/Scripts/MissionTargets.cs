using UnityEngine;
using System.Collections;

[System.Serializable]
public class MissionTargets{

    public GameObject Target;
    public string MissionDescription;

    public MissionTargets(GameObject target, string missionDescription) {
        Target = target;
        MissionDescription = missionDescription;
    }
}
