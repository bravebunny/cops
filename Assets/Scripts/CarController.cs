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

    private float HydrantStrenght = 100;
    private bool InHydrant = false;
    private Vector3 HydrantPosition;
    private float TorqueForce = 0, TorqueVelocity = 0;

    private AudioSource EngineSound;


    public float carMovementDirection = 0;

    // Use this for initialization
    void Start () {
        Body = GetComponent<Rigidbody>();
        EngineSound = GetComponent<AudioSource>();
        Body.centerOfMass = Vector3.down;
    }

    void OnCollisionEnter(Collision collision) {
        string tag = collision.gameObject.tag;
        float speed = collision.relativeVelocity.magnitude;
        if (tag == "FireHydrant" && speed > 25) {
            collision.gameObject.tag = "Untagged";
            collision.rigidbody.isKinematic = false;
            collision.rigidbody.centerOfMass = Vector3.forward;
            collision.rigidbody.AddForce(-collision.relativeVelocity * 0.01f + Vector3.up * 0.5f, ForceMode.Impulse);
            collision.rigidbody.AddTorque(Quaternion.Euler(0, 90, 0) * collision.relativeVelocity);
            GameObject water = collision.transform.parent.FindChild("Water").gameObject;
            StartCoroutine(ActivateWithDelay(water, 0.3f));
        }
    }

    IEnumerator ActivateWithDelay(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }

    void OnTriggerEnter(Collider collider) {
        string tag = collider.gameObject.tag;
        if (tag == "Destructible") {
            foreach (Transform child in collider.transform) {
                //enable physics
                child.GetComponent<Rigidbody>().isKinematic = false;

                //enable collisions if usnig box collider
                if (child.GetComponent<BoxCollider>())
                    child.GetComponent<BoxCollider>().enabled = true;
            }
        } else if (tag == "FireHydrant") {
            InHydrant = true;
            HydrantPosition = collider.transform.position;
        }
    }

    void OnTriggerExit(Collider collider) {
        string tag = collider.gameObject.tag;
        if (tag == "FireHydrant") {
            InHydrant = false;
        }
    }

    public void Update() {

    }

    public void FixedUpdate() {
        if (InHydrant) Body.AddForceAtPosition(Vector3.up * HydrantStrenght, HydrantPosition);
    }

    public void Move (float steering, float accel) {
        if (Body == null)
            return;

        EngineSound.volume = CurrentSpeed / 60 - 0.1f;
        EngineSound.pitch = (CurrentSpeed / 60) * 6 - 3;

        RaycastHit raycastInfo = new RaycastHit();
        float raycastDistance = SuspensionHeight + 1f;
        bool grounded = Physics.Raycast(Body.position, -transform.up, out raycastInfo, raycastDistance);
        bool notClimbing = Physics.Raycast(Body.position, -Vector3.up, out raycastInfo, raycastDistance);

        if (DebugOn) Debug.DrawRay(Body.position, -transform.up, Color.red, -1, false);

        if (accel != 0) DirectionVal = (int)(accel / Mathf.Abs(accel));
        else DirectionVal = 1;

        Body.AddRelativeTorque(0, steering * TurningSpeed * DirectionVal, 0);

        if (grounded && notClimbing) {
            Body.drag = Drag;

            Vector3 velocity = Body.velocity;
            float sidewaysVelocity = transform.InverseTransformDirection(Body.velocity).x;
            carMovementDirection = transform.InverseTransformDirection(Body.velocity).z;

            Vector3 force = transform.forward * accel * Speed;
            //Vector3 force = transform.rotation * new Vector3(accel * Speed, 0, -sidewaysVelocity * SidewaysCompensation);
            Vector3 groundNormal = raycastInfo.normal;
            if (DebugOn) Debug.DrawRay(raycastInfo.point, groundNormal, Color.green, -1, false);

            Vector3 projectedForce = Vector3.ProjectOnPlane(force, groundNormal);

            Vector3 forcePosition = Body.position + transform.rotation * new Vector3(-2 * DirectionVal, 0, 0);

            if (DebugOn) Debug.DrawLine(forcePosition, Body.position, Color.black, -1, false);
            if (DebugOn) Debug.DrawRay(forcePosition, projectedForce, Color.blue, -1, false);

            Body.AddForce(projectedForce);
            float maxTorque = 100;
            if (accel == 0) {
                TorqueForce = 0;
                TorqueVelocity = 0;
            }
            TorqueForce = Mathf.SmoothDamp(TorqueForce, maxTorque, ref TorqueVelocity, 0.2f);
            float torqueStrength = (maxTorque - TorqueForce) * -accel;
            Body.AddRelativeTorque(Vector3.right * torqueStrength, ForceMode.Acceleration);

            Blocked = (velocity.magnitude < 1 && force.magnitude >= 1);
        } else {
            Body.drag = 0;
        }

        //distance of the wheels to the car
        Vector3 xDist = transform.forward;
        Vector3 zDist = -transform.right * 0.8f;
        Suspension(2, Body.position + (xDist + zDist));
        Suspension(3, Body.position + (-xDist + zDist));
        Suspension(0, Body.position + (xDist - zDist));
        Suspension(1, Body.position + (-xDist - zDist));
    }


    void Suspension (int index, Vector3 origin) {
        Vector3 direction = -transform.up;
        RaycastHit info = new RaycastHit();
        bool grounded = Physics.Raycast(origin, direction, out info, SuspensionHeight);
        Transform wheel = transform.GetChild(0).FindChild("Wheel" + index);

        if (grounded) {
            float wheelHeight = 0.25f;
            wheel.position = new Vector3(info.point.x, info.point.y + wheelHeight, info.point.z);
            float strength = SuspensionStrength / (info.distance / SuspensionHeight) - SuspensionStrength;
            strength = Mathf.Min(strength, 30);
            Vector3 push = Vector3.up * strength;
            Body.AddForceAtPosition(push, origin);
        } else {
            wheel.position = origin + direction * SuspensionHeight * 0.75f;
        }
       
        wheel.Rotate(new Vector3(0,0, transform.InverseTransformDirection(Body.velocity).z * 0.5f));

        if (DebugOn) {
            if (grounded) {
                Debug.DrawLine(origin, info.point, Color.white, -1, false);
            } else {
                Debug.DrawLine(origin, origin + direction * SuspensionHeight, Color.white, -1, false);
            }
        }
    }
}
