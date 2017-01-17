using UnityEngine;
using System.Collections;

public class MissionField : MonoBehaviour {

    public Transform WantedParent;

	void Start () {
        gameObject.transform.parent = WantedParent;
    }
	
}
