using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CarController : MonoBehaviour {
    public bool playerControlled = false;

    private Rigidbody body;

	// Use this for initialization
	void Start () {
         body = GetComponent< Rigidbody > ();
    }
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

       Move(h, v, v);
    }

    void Move (float steering, float accel, float footbrake) {

        Vector3 velocity = body.velocity;
        float sidewaysVelocity = transform.InverseTransformDirection(body.velocity).z;

        body.AddRelativeForce(accel * 10, 0, -sidewaysVelocity * 30);
        body.AddRelativeForce(accel * 10, 0, 0);
        body.AddRelativeTorque(0, steering, 0);

    }
}
