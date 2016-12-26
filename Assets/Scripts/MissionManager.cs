using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {

    public GameObject MissionObject;
    public Text DisplayText;
    [HideInInspector]
    public Targets TargetScript;

    MissionsAbstract RandomMission;
    MissionsAbstract[] MissionsList;

    void Awake() {
        MissionsList = MissionObject.GetComponents<MissionsAbstract>();
        TargetScript = MissionObject.GetComponent<Targets>();
    }

    // Use this for initialization
    void Start () {
        RandomizeMission();
    }


    public void RandomizeMission() {
        RandomMission = MissionsList[Random.Range(0, MissionsList.Length)];
        RandomMission.InitiateMission(this);
        setDisplayText();
    }

    public void EndCurrentMission(bool nextPhase = false, int targetIndex = 0) {
        if (nextPhase) {
            RandomMission.InitiateMission(this, targetIndex + 1);
            setDisplayText();
        } else RandomizeMission();
    }

     void setDisplayText() {
        DisplayText.enabled = true;
        DisplayText.text = RandomMission.GetDisplayText();
    }
}
