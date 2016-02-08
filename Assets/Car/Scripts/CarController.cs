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
        Body.centerOfMass.Set(0.5f, -0.3f, 0);
    }
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

       Move(h, v);
    }

    void Move (float steering, float accel) {
        

        RaycastHit raycastInfo = new RaycastHit();
        float raycastDistance = 2;
        bool grounded = Physics.Raycast(Body.position, transform.rotation * -transform.up, out raycastInfo, raycastDistance);

        if (grounded) {
            Vector3 velocity = Body.velocity;
            float sidewaysVelocity = transform.InverseTransformDirection(Body.velocity).z;

            Vector3 force = new Vector3(accel * Speed, 0, -sidewaysVelocity * SidewaysCompensation);
            Vector3 groundNormal = raycastInfo.normal;

            Vector3 projectedForce = transform.rotation * Vector3.ProjectOnPlane(force, groundNormal);

            //Body.AddForceAtPosition();
            Body.AddForce(projectedForce);
            Vector3 forcePosition = Body.position + transform.rotation * new Vector3(0.5f, -0.2f, 0);
            //Body.AddForceAtPosition(projectedForce, forcePosition);

            Body.AddRelativeTorque(0, steering * TurningSpeed, 0);

            Suspension(Body.position + transform.rotation * new Vector3(1, 0, 1));
            Suspension(Body.position + transform.rotation * new Vector3(-1, 0, 1));
            Suspension(Body.position + transform.rotation * new Vector3(1, 0, -1));
            Suspension(Body.position + transform.rotation * new Vector3(-1, 0, -1));
        }
    }


    void Suspension (Vector3 origin) {
        Vector3 direction = -transform.up;
        RaycastHit info = new RaycastHit();
        float maxDistance = 1;

        Physics.Raycast(origin, direction, out info, maxDistance);
        float strength = SuspensionStrength / info.distance - SuspensionStrength;
        Vector3 push = transform.rotation * new Vector3(0, strength, 0);

        //Body.AddForce(0, push, 0);
        Body.AddForceAtPosition(push, origin);
    }
}
