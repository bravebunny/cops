using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    Vector3 InitialPoint;
    Vector3 Position;
    Vector3 Point;
	// Use this for initialization
	void Start () {
        InitialPoint = transform.position;
        Position = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //transform.RotateAround(InitialPoint + Vector3.up, -Vector3.forward, 2);

        Position = Quaternion.AngleAxis(0.002F, Vector3.forward) * Position;
        transform.position = Point + Position;
    }
}
