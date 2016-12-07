using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
    public LookAt Arrow;
    public Transform TargetMarker;
    public float MarkerHeight = 2;
    Transform Target;
    AudioSource CollectSound;
    MissionsAbstract CurrentMission;
    bool ArrowDisplay = false; //show Arrow on Screen 


    void Start() {
        CollectSound = GetComponent<AudioSource>();
        Debug.Log(CollectSound + "SOUNDDDDDDDDDDd");
	}

    public void SetMission(MissionsAbstract currentMission, bool arrowDisplay) {
        CurrentMission = currentMission;
        ArrowDisplay = arrowDisplay;

        //show Arrow on Screen 
        if (ArrowDisplay) {
            TargetMarker.gameObject.SetActive(true);
            Arrow.gameObject.gameObject.SetActive(true);
        }
    }

    public void NewTarget(GameObject target) {

        // pick new target randomly from children
        int index = Random.Range(0, target.transform.childCount);
        Target = target.transform.GetChild(index);

        // add goal behavior to new target
        Goal goal = Target.gameObject.AddComponent<Goal>();
        goal.TargetManager = this;

        // add marker that follows target around
        TargetMarker.position = Target.position + Vector3.up * MarkerHeight;
        TargetMarker.SetParent(Target);

        // make the arrow point to the new target
        Arrow.Target = Target;

    }

    public void DestroyTarget() {
        // remove goal behavior from previous target
        //CollectSound.Play();
        Destroy(Target.GetComponent<Goal>());
        CurrentMission.EndMission();

        TargetMarker.gameObject.SetActive(false); //probabily remove this after
        Arrow.gameObject.gameObject.SetActive(false); //probabily remove this after
    }
}
