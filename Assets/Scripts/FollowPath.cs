using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
    public float Speed = 5;
    public float MaxGroundDistance = 0.5f;
    public float RotationSpeed = 150;
    public float LaneWidth = 2.5f;
    float ForwardCheckLength = 4f;
    Rigidbody Body;
    Road CurrentRoad;
    [HideInInspector] public Vector3 Target;

	void Start() {
        Body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate() {
        MoveTowardsTarget();
        if (IsBlocked()) {
            Body.velocity = Vector3.zero;
            Target -= transform.forward * 3;
        }
    }

    void MoveTowardsTarget() {
        Vector3 target = Target + transform.right * LaneWidth;
        Vector3 targetDirection;
        Vector3 transformForward = transform.forward;
        transformForward.y = 0;
        Vector3 forward;

        bool targetBehind = Util.IsInFront(transform.position + transformForward * 2, target, transformForward);
        if (targetBehind) {
            targetDirection = -transform.right;
            forward = transformForward;
        } else {
            targetDirection = Vector3.ProjectOnPlane((target - transform.position), Vector3.up);
            forward = targetDirection;
        }

        Debug.DrawLine(transform.position, transform.position + forward, Color.blue, -1, false);

        Body.AddForce(forward.normalized * Speed, ForceMode.VelocityChange);
        Body.angularVelocity = Vector3.zero;
        Body.AddTorque(Vector3.Cross(transformForward, targetDirection) * RotationSpeed, ForceMode.Acceleration);
        Body.AddTorque(Vector3.Cross(transform.up, Vector3.up) * RotationSpeed);
    }

    // pick one of the connections randomly
    Vector3 PickTarget() {
        List<Transform> connections = CurrentRoad.Connections;
        if (connections.Count == 1) return CurrentRoad.Connections[0].position;

        List<Vector3> targets = new List<Vector3>();
        foreach (Transform connection in CurrentRoad.Connections) {
            bool inFront = Util.IsInFront(connection.position, transform.position - transform.forward * 2, transform.forward);
            if (inFront) targets.Add(connection.position);
        }
        int count = targets.Count;
        if (count == 0) {
            return Target;
         }
        int index = Random.Range(0, targets.Count);
        return targets[index];
    }

    bool IsBlocked() {
        RaycastHit hit;
        Vector3 origin = transform.position + transform.forward + Vector3.up;
        Vector3 direction = Vector3.ProjectOnPlane(transform.forward, Vector3.up); ;
        Debug.DrawLine(origin, origin + direction * ForwardCheckLength);
        bool ray = Physics.Raycast(origin, direction, out hit, ForwardCheckLength);
        return ray;
    }

    void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("Road")) return;
        Debug.Log("found road");
        CurrentRoad = col.gameObject.GetComponent<Road>();
        Target = PickTarget();
    }
}
