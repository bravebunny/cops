using UnityEngine;
using System.Collections;

public class HelicopterController : MonoBehaviour {
    public float Speed = 50;
    public float TurningSpeed = 5;
    public float AttackSpeed = 10;
    public float RotorBaseSpeed = 20;
    public float RotorSpeed = 10;
    public float BackRotorSpeed = 30;
    public float SmoothSpeed = 20;

    private Rigidbody Body;
    private Rigidbody Rotor;
    private Rigidbody BackRotor;

    // Use this for initialization
    void Start () {
        Body = GetComponent<Rigidbody>();
        Rotor = transform.FindChild("Rotor").GetComponent<Rigidbody>();
        BackRotor = transform.FindChild("BackRotor").GetComponent<Rigidbody>();
    }

    public void Move (float lift, float steering, float attack) {
        if (Body == null)
            return;

        Vector3 relativeTorque = new Vector3(0f, steering * TurningSpeed, attack * AttackSpeed);

        float xCompensation = (transform.eulerAngles.x / 360);
        if (xCompensation > 0.5)
            xCompensation = xCompensation - 1;
        xCompensation *= 4;

        relativeTorque.x = -xCompensation * SmoothSpeed;

        if (attack == 0) {
            float zCompensation = (transform.eulerAngles.z / 360);
            if (zCompensation > 0.5)
                zCompensation = zCompensation - 1;
            zCompensation *= 4;

            relativeTorque.z = -zCompensation * SmoothSpeed;
        }

        Body.AddRelativeTorque(relativeTorque);

        Vector3 relativeForce = new Vector3(0f, lift * Speed, 0f);

        if (lift == 0) {
            relativeForce.y = - Physics.gravity.y - Body.velocity.y * Speed;
        }

        Body.AddRelativeForce(relativeForce);

        Rotor.transform.eulerAngles = new Vector3(Rotor.transform.eulerAngles.x, Rotor.transform.eulerAngles.y, Rotor.transform.eulerAngles.z - (RotorBaseSpeed + lift * RotorSpeed));
        BackRotor.transform.eulerAngles = new Vector3(BackRotor.transform.eulerAngles.x, BackRotor.transform.eulerAngles.y, BackRotor.transform.eulerAngles.z - (steering * BackRotorSpeed));
    }
}
