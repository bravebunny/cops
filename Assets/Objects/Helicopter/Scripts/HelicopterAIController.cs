using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class HelicopterAIController : MonoBehaviour {
    public float TargetHeight = 3f;

    [SerializeField] private Transform Target;

    private HelicopterController HelicopterController;
    private Rigidbody Rigidbody;

    private void Awake()
    {
        // get the heli controller
        HelicopterController = GetComponent<HelicopterController>();

        // Allows to disable camera collisions with cops
        HelicopterController.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
    }

    void FixedUpdate () {
        if (Target == null) {
            HelicopterController.Move(0, 0, 0);
            return;
        }

        Vector3 offsetTargetPos = Target.position;
        float dist = Vector2.Distance(new Vector2(offsetTargetPos.x, offsetTargetPos.z), new Vector2(transform.position.x, transform.position.z));

        // calculate the local-relative position of the target, to steer towards
        Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

        // work out the local angle towards the target
        float targetAngle = Mathf.Atan2(localTarget.z, localTarget.x) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle, -1, 1);

        Vector3 direction = -transform.up;
        RaycastHit info = new RaycastHit();
        bool grounded = Physics.Raycast(transform.position, direction, out info, TargetHeight * 2);

        float lift = 0f;
        if (dist < TargetHeight) {
            lift = -1f;
        } else if (info.distance == 0)
            lift = -1f;
        else if (info.distance < TargetHeight) {
            lift = 1f;
        }

        float attack = Mathf.Clamp(dist, -1f, 1f);
        if (steer < 0)
            attack = 0;

        HelicopterController.Move(lift, steer, attack);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
