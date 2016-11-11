using UnityEngine;
using System.Collections;

public class LookAtDelay : MonoBehaviour {

    private Transform target;
    public float speed = 0.5F;
    public float lookAtDistance = 1.0F;

    // Use this for initialization
    void Start () {
        target = GameManager.Player.transform;
    }
	
	void Update () {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < lookAtDistance) {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
}
