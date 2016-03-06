using UnityEngine;
using System.Collections;

public class HelicopterController : MonoBehaviour {
    public float Speed = 100;
    public float TurningSpeed = 100;
    public float AttackSpeed = 100;
    public float RotorBaseSpeed = 20;
    public float RotorSpeed = 20;
    public float BackRotorSpeed = 50;
    public float SmoothSpeed = 20f;

    private Rigidbody Body;
    private Rigidbody Rotor;
    private Rigidbody BackRotor;

    private Vector3 camVel = Vector3.zero;

    // Use this for initialization
    void Start () {
        Body = GetComponent<Rigidbody>();
        Rotor = transform.FindChild("Rotor").GetComponent<Rigidbody>();
        BackRotor = transform.FindChild("BackRotor").GetComponent<Rigidbody>();
    }

    public void Move (float lift, float steering, float attack) {
        if (Body == null)
            return;

        float xCompensation = (transform.eulerAngles.x / 360);
        if (xCompensation > 0.5)
            xCompensation = xCompensation - 1;
        xCompensation *= 4;

        Body.AddRelativeTorque(-xCompensation * SmoothSpeed, steering * TurningSpeed, attack * AttackSpeed);

        Body.AddRelativeForce(0f, lift * Speed, 0f);

        Rotor.transform.eulerAngles = new Vector3(Rotor.transform.eulerAngles.x, Rotor.transform.eulerAngles.y, Rotor.transform.eulerAngles.z - (RotorBaseSpeed + lift * RotorSpeed));
        BackRotor.transform.eulerAngles = new Vector3(BackRotor.transform.eulerAngles.x, BackRotor.transform.eulerAngles.y, BackRotor.transform.eulerAngles.z - (steering * BackRotorSpeed));
    }
}
