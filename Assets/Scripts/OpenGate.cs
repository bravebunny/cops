using UnityEngine;
using System.Collections;

public class OpenGate : MonoBehaviour {
    public float Speed = 0.1F;
    public float timeLeft = 5;
    float initialTimeLeft = 5;

    Quaternion desireRotation = Quaternion.identity;
    bool Close = false;

    // Use this for initialization
    void Start () {
        desireRotation.eulerAngles = new Vector3(0, 0, 0);
    }

    void FixedUpdate() {
        if (Close) {
            desireRotation.eulerAngles = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, desireRotation, Speed);

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                Close = false;
            }

        } else {
            desireRotation.eulerAngles = new Vector3(0, 0, 35);
            transform.rotation = Quaternion.Lerp(transform.rotation, desireRotation, Speed);
        }

    }

    void OnTriggerEnter(Collider collider) {
       if (collider.GetComponentInParent<Train>()) {
            timeLeft = initialTimeLeft;
            Close = true; 
       }
    }
}
