using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public Targets TargetManager;
    AudioSource CollectSound;

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player") {
            TargetManager.NewTarget();
            GameManager.Score++;
            GameManager.KilledCops++;
        }
    }

    void OnDestroy() {
        TargetManager.NewTarget();
    }
}
