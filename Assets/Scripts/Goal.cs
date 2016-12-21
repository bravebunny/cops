using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public Targets TargetManager;

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player") {
            GameManager.Score++;
            GameManager.KilledCops++;
            TargetManager.DestroyTarget();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            GameManager.Score++;
            GameManager.KilledCops++;
            TargetManager.DestroyTarget();
        }
    }

    void OnDestroy() {
        //TargetManager.NewTarget();
    }
}
