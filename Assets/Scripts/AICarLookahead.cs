using UnityEngine;
using System.Collections;

public class AICarLookahead : MonoBehaviour {
    FollowPath Parent;

	void Start () {
        Parent = transform.parent.GetComponent<FollowPath>();
	}
	
	void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return;
        Parent.Reverse();
    }

    void OnTriggerExit(Collider col) {
        Parent.Blocked = false;
    }
}
