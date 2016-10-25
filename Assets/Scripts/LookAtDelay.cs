using UnityEngine;
using System.Collections;

public class LookAtDelay : MonoBehaviour {

    public Transform target;
    public float speed = 0.5F;
    public float lookAtDistance = 1.0F;

    // Use this for initialization
    void Start () {}
	
	void Update () {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < lookAtDistance) {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
}
