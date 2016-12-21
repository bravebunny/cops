using UnityEngine;
using System.Collections;

public class MissionField : MonoBehaviour {

    public Transform WantedParent;

	// Use this for initialization
	void Awake () {
        gameObject.transform.parent = WantedParent;
    }
	
}
