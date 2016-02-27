using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
    public float Speed = 20;
    public float SidewaysCompensation = 0;
    public float SuspensionStrength = 5;
    public float TurningSpeed = 2;
    public bool DebugOn = true;
    public float SuspensionHeight = 1;
    public float Drag = 5;

    [HideInInspector] public bool Blocked = false;
    public float CurrentSpeed{ get { return Body.velocity.magnitude*2.23693629f; }}

    private Rigidbody Body;
    private int DirectionVal;
    private float AngleZ = 0;
    private float AngleX = 0;
    private float RotationDirection;

    // Use this for initialization
    void Start () {
        Body = GetComponent<Rigidbody>();
        Body.centerOfMass = new Vector3(0, -1, 0);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag.Equals("Destructible")) {
            foreach (Transform child in collider.transform) {
                //enable physics
                child.GetComponent<Rigidbody>().isKinematic = false;

                //enable collisions if usnig box collider
                if (child.GetComponent<BoxCollider>())
                    child.GetComponent<BoxCollider>().enabled = true;

                //enable collisions if using mesh collider
                /*else if (child.GetComponent<MeshCollider>())
                    child.GetComponent<MeshCollider>().enabled = true;*/
            }
        }
    }

    public void Move (float steering, float accel) {
        if (Body == null)
            return;

        RaycastHit raycastInfo = new RaycastHit();
        float raycastDistance = SuspensionHeight + 1f;
        bool grounded = Physics.Raycast(Body.position, -transform.up, out raycastInfo, raycastDistance);
        bool notClimbing = Physics.Raycast(Body.position, -Vector3.up, out raycastInfo, raycastDistance);

        if (DebugOn) Debug.DrawRay(Body.position, -transform.up, Color.red, -1, false);

        if (accel != 0) DirectionVal = (int)(accel / Mathf.Abs(accel));
        else DirectionVal = 1;

        Body.AddRelativeTorque(0, steering * TurningSpeed * DirectionVal, 0);

        if (grounded) {
            Body.drag = Drag;

            Vector3 velocity = Body.velocity;
            float sidewaysVelocity = transform.InverseTransformDirection(Body.velocity).z;

            Vector3 force = transform.right * accel * Speed;
            //Vector3 force = transform.rotation * new Vector3(accel * Speed, 0, -sidewaysVelocity * SidewaysCompensation);
            Vector3 groundNormal = raycastInfo.normal;
            if (DebugOn) Debug.DrawRay(raycastInfo.point, groundNormal, Color.green, -1, false);

            Vector3 projectedForce = Vector3.ProjectOnPlane(force, groundNormal);

            Vector3 forcePosition = Body.position + transform.rotation * new Vector3(-2 * DirectionVal, 0, 0);

            if (DebugOn) Debug.DrawLine(forcePosition, Body.position, Color.black, -1, false);
            if (DebugOn) Debug.DrawRay(forcePosition, projectedForce, Color.blue, -1, false);

            Body.AddForce(projectedForce);

            Blocked = (velocity.magnitude < 1 && force.magnitude >= 1);
        } else {
            Body.drag = 0;
            /*Vector3 angles = transform.eulerAngles;
            float angleZ = angles.z;
            float angleX = angles.x;
            //if (angleZ > 350 || angleZ < 60) Body.AddRelativeTorque(0, 0, -20);
            
            float newAngleZ = Mathf.SmoothDampAngle(angleZ, -25, ref AngleZ, 1f);
            float newAngleX = Mathf.SmoothDampAngle(angleX, 0, ref AngleX, 1f);

            transform.eulerAngles = new Vector3 (newAngleX, angles.y, newAngleZ);*/
        }

        //distance of the wheels to the car
        float xDist = 1.2f;
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
            strength = Mathf.Min(strength, 30);
            Vector3 push = transform.up * strength;
            Body.AddForceAtPosition(push, origin);
        } else {
            wheel.position = origin + direction * SuspensionHeight * 0.75f;
        }
       
        wheel.Rotate(new Vector3(0,0, transform.InverseTransformDirection(Body.velocity).x * 0.5f));

        if (DebugOn) {
            if (grounded) {
                Debug.DrawLine(origin, info.point, Color.white, -1, false);
            } else {
                Debug.DrawLine(origin, origin + direction * SuspensionHeight, Color.white, -1, false);
            }
        }
    }
}
