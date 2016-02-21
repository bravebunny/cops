using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    public float Speed = 20;
    public float SidewaysCompensation = 0;
    public float SuspensionStrength = 5;
    public float TurningSpeed = 2;
    public bool DebugOn = true;
    public float SuspensionHeight = 3;
    public float Drag = 5;

    [HideInInspector] public bool Blocked = false;
    public float CurrentSpeed{ get { return Body.velocity.magnitude*2.23693629f; }}

    private Rigidbody Body;
    private int directionVal;

    private bool Finish = false;

    // Use this for initialization
    void Start () {
        Body = GetComponent<Rigidbody>();
        Body.centerOfMass = new Vector3(0, -1, 0);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Garage") == true)
        {
            var VecDist = collision.transform.position - Body.position;
            if (VecDist.magnitude <= 3) {
                Finish = true;
                Debug.Log("Inside Garage");
            }
            // if (DebugOn)  Debug.Log("Distance To Garage: " + VecDist.magnitude);
        }
    }

    public void Move (float steering, float accel) {
        RaycastHit raycastInfo = new RaycastHit();
        float raycastDistance = SuspensionHeight + 0.5f;
        bool grounded = Physics.Raycast(Body.position, -transform.up, out raycastInfo, raycastDistance);

        if (DebugOn) Debug.DrawRay(Body.position, -transform.up, Color.red, -1, false);

        if (accel != 0) directionVal = (int)(accel / Mathf.Abs(accel));
        else directionVal = 1;

        Body.AddRelativeTorque(0, steering * TurningSpeed * directionVal, 0);

        if (grounded) {
            Body.drag = 5;

            Vector3 velocity = Body.velocity;
            float sidewaysVelocity = transform.InverseTransformDirection(Body.velocity).z;

            Vector3 force = transform.rotation * new Vector3(accel * Speed, 0, -sidewaysVelocity * SidewaysCompensation);
            Vector3 groundNormal = raycastInfo.normal;
            if (DebugOn) Debug.DrawRay(raycastInfo.point, groundNormal, Color.green, -1, false);

            Vector3 projectedForce = Vector3.ProjectOnPlane(force, groundNormal);

            Vector3 forcePosition = Body.position + transform.rotation * new Vector3(2 * directionVal, -1.5f, 0);

            if (DebugOn) Debug.DrawLine(forcePosition, Body.position, Color.black, -1, false);
            if (DebugOn) Debug.DrawRay(forcePosition, projectedForce, Color.blue, -1, false);

            Body.AddForceAtPosition(projectedForce, forcePosition);

            Blocked = (velocity.magnitude < 1 && force.magnitude >= 1);

            if (DebugOn && Blocked) Debug.Log("Blocked");
        } else {
            Body.drag = 0;
        }

        //distance of the wheels to the car
        float xDist = 1f;
        float yDist = 0.8f;
        Suspension(3, Body.position + transform.rotation * new Vector3(xDist, 0, yDist));
        Suspension(4, Body.position + transform.rotation * new Vector3(-xDist, 0, yDist));
        Suspension(1, Body.position + transform.rotation * new Vector3(xDist, 0, -yDist));
        Suspension(2, Body.position + transform.rotation * new Vector3(-xDist, 0, -yDist));
    }


    void Suspension (int index, Vector3 origin) {
        Vector3 direction = -transform.up;
        RaycastHit info = new RaycastHit();
        bool grounded = Physics.Raycast(origin, direction, out info, SuspensionHeight);
        Transform wheel = transform.GetChild(0).GetChild(index);

        if (grounded) {
            float wheelHeight = 0.25f;
            wheel.position = new Vector3(info.point.x, info.point.y + wheelHeight, info.point.z);
            float strength = SuspensionStrength / (info.distance / SuspensionHeight) - SuspensionStrength;
            Vector3 push = transform.rotation * new Vector3(0, strength, 0);
            Body.AddForceAtPosition(push, origin);
        } else {
            wheel.position = origin + direction * SuspensionHeight * 0.75f;
        }
       
        wheel.Rotate(new Vector3(0,0, CurrentSpeed * 0.5f * directionVal));

        if (DebugOn) {
            if (grounded) {
                Debug.DrawLine(origin, info.point, Color.white, -1, false);
            } else {
                Debug.DrawLine(origin, origin + direction * SuspensionHeight, Color.white, -1, false);
            }
        }
    }
}
