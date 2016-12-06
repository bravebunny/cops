using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {

    public GameObject MissionObject;
    public Text DisplayText;

    static MissionsAbstract RandomMission;
    static MissionsAbstract[] MissionsList;

    // Use this for initialization
    void Start () {
        MissionsList = MissionObject.GetComponents<MissionsAbstract>();
        RandomizeMission();
    }


    public static void RandomizeMission() {
        Debug.Log("##RANDOMMMMM##");
        RandomMission = MissionsList[Random.Range(0, MissionsList.Length)];
        RandomMission.InitiateMission();
        //setDisplayText();
    }

    public static void EndCurrentMission() {
        RandomMission.EndMission();
        RandomizeMission();
    }

     void setDisplayText() {
        DisplayText.enabled = true;
        DisplayText.text = RandomMission.GetDisplayText();
    }
}
