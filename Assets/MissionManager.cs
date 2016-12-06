using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {

    public GameObject MissionObject;
    public Text DisplayText;

    MissionsAbstract RandomMission;
    MissionsAbstract[] MissionsList;

    // Use this for initialization
    void Start () {
        MissionsList = MissionObject.GetComponents<MissionsAbstract>();
        RandomizeMission();
    }


    public void RandomizeMission() {
        Debug.Log("##RANDOMMMMM##");
        RandomMission = MissionsList[Random.Range(0, MissionsList.Length)];
        RandomMission.InitiateMission(this);
        setDisplayText();
    }

    public void EndCurrentMission() {
        RandomizeMission();
    }

     void setDisplayText() {
        DisplayText.enabled = true;
        DisplayText.text = RandomMission.GetDisplayText();
    }
}
