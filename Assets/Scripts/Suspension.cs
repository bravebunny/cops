using UnityEngine;
using System.Collections;

public class Suspension : MonoBehaviour {
    public float SuspensionHeight;
    public float SuspensionStrength;
    public LayerMask CollisionLayers;
    bool DebugOn = true;
    Rigidbody CarBody;
    Transform CarTransform;
    Vector3 OriginOffset;
    float WheelHeight;

	// Use this for initialization
	void Start () {
        CarBody = GetComponentInParent<Rigidbody>();
        CarTransform = CarBody.transform;
        OriginOffset = transform.position - CarTransform.position;
        //OriginOffset = CarTransform.rotation * OriginOffset;
        OriginOffset = CarTransform.InverseTransformDirection(OriginOffset);
    }

    // Update is called once per frame
    void FixedUpdate () {
        float wheelHeight = 0.25f;
        Vector3 direction = -CarTransform.up;
        RaycastHit info = new RaycastHit();
        Vector3 xDist = OriginOffset.z * CarTransform.forward;
        Vector3 zDist = OriginOffset.x * CarTransform.right;
        Vector3 yDist = (OriginOffset.y) * CarTransform.up;
        Vector3 origin = CarTransform.position + xDist + zDist + yDist/1.5f;
        bool grounded = Physics.Raycast(origin, direction, out info, SuspensionHeight, CollisionLayers);

        if (grounded) {
            transform.position = new Vector3(info.point.x, info.point.y + wheelHeight, info.point.z);
            float strength = SuspensionStrength / (info.distance / SuspensionHeight) - SuspensionStrength;
            strength = Mathf.Min(strength, 30);
            Vector3 push = Vector3.up * strength;
            CarBody.AddForceAtPosition(push, origin);
        } else {
            transform.position = origin + direction * SuspensionHeight * 0.75f;
        }

        transform.Rotate(new Vector3(0, 0, CarTransform.InverseTransformDirection(CarBody.velocity).z * 0.5f));

        if (DebugOn) {
            if (grounded) {
                Debug.DrawLine(origin, info.point, Color.white, -1, false);
            } else {
                Debug.DrawLine(origin, origin + direction * SuspensionHeight, Color.white, -1, false);
            }
        }
    }
}
