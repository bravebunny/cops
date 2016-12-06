using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public Targets TargetManager;

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player") {
            MissionManager.EndCurrentMission();
            GameManager.Score++;
            GameManager.KilledCops++;
        }
    }

    void OnDestroy() {
        //TargetManager.NewTarget();
    }
}
