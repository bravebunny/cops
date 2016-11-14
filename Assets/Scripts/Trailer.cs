using UnityEngine;
using System.Collections;

public class Trailer : MonoBehaviour {

	void Start () {
        GetComponent<HingeJoint>().connectedBody = GameManager.Player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
