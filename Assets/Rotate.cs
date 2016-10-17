using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    public Vector3 Speeds;
	
	void FixedUpdate () {
        transform.eulerAngles += Speeds;
	}
}
