using UnityEngine;
using System.Collections;

public class Corn : MonoBehaviour {

    bool collisionOn;
    bool checkTrigger = true;
    Vector3 heading;
    Vector3 direction;
    public float speed = 0.01F;

    // Use this for initialization
    void Start () {
        collisionOn = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (collisionOn) {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, speed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir.normalized);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Player" && checkTrigger) {
            checkTrigger = false;
            heading = coll.transform.position - transform.position;
            direction = heading.normalized;
            collisionOn = true;
        }
       
    }
}
