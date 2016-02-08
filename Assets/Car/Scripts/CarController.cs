using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CarController : MonoBehaviour {
    public bool playerControlled = false;
    public float Speed = 20;
    public float SidewaysCompensation = 30;
    public float SuspensionStrength = 5;
    public float TurningSpeed = 2;

    private Rigidbody Body;

	// Use this for initialization
	void Start () {
         Body = GetComponent< Rigidbody > ();
    }
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

       Move(h, v);
    }

    void Move (float steering, float accel) {

        Vector3 velocity = Body.velocity;
        float sidewaysVelocity = transform.InverseTransformDirection(Body.velocity).z;

        //Body.AddForceAtPosition();
        Body.AddRelativeForce(accel * Speed, 0, -sidewaysVelocity * SidewaysCompensation);
        Body.AddRelativeTorque(0, steering * TurningSpeed, 0);

        Suspension(Body.position + transform.rotation * new Vector3(1, 0, 1));
        Suspension(Body.position + transform.rotation * new Vector3(-1, 0, 1));
        Suspension(Body.position + transform.rotation * new Vector3(1, 0, -1));
        Suspension(Body.position + transform.rotation * new Vector3(-1, 0, -1));
    }


    void Suspension (Vector3 origin) {
        Vector3 direction = new Vector3(0, -1, 0);
        RaycastHit info = new RaycastHit();
        float maxDistance = 1;

        Physics.Raycast(origin, direction, out info, maxDistance);
        Debug.Log(info.distance);
        Vector3 push = new Vector3(0, SuspensionStrength / info.distance, 0);

        //Body.AddForce(0, push, 0);
        Body.AddForceAtPosition(push, origin);
    }
}
