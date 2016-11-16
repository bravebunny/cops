using UnityEngine;
using System.Collections;

public class SkipOnWater : MonoBehaviour {
    public float MinY = -4;
    public float MinVelocity = 10;
    public float ImpulseForce = 10;
    public float Resistance = 5;
    public Object Splash;
    Rigidbody Body;

    void Start() {
        Body = GetComponent<Rigidbody>();
    }

	void Update () {
        Vector3 velocity = new Vector3(Body.velocity.x, 0, Body.velocity.z) / 2;
        if (transform.position.y < MinY && velocity.magnitude > MinVelocity) {
            Vector3 newPosition = transform.position;
            newPosition.y = MinY;
            transform.position = newPosition;
            Vector3 force = (Vector3.down * ImpulseForce * Body.velocity.y) - (velocity.normalized * Resistance);
            Vector3 forcePosition = transform.position - velocity.normalized;
            Body.AddForceAtPosition(force, forcePosition, ForceMode.Impulse);
            Instantiate(Splash, transform.position, Quaternion.identity);
        }
	}
}
