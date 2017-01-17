using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
    public float Speed = 5;
    public float RotationSpeed = 150;
    public float LaneWidth = 2.5f;

    float ForwardCheckLength = 4f;
    Rigidbody Body;
    Road CurrentRoad;
    [HideInInspector] public Vector3 Target;
    [HideInInspector] public bool Blocked;

    void Start() {
        Body = GetComponent<Rigidbody>();
        transform.parent = GameManager.TrafficCars.transform;
	}
	
	void FixedUpdate() {
        MoveTowardsTarget();
    }

    public void Reverse() {
        Body.velocity = Vector3.zero;
        Target -= transform.forward * 3;
    }

    Vector3 transformForward;
    Vector3 transformRight;
    Vector3 target;
    Vector3 position;
    Vector3 targetDirection;
    Vector3 forward;
    void MoveTowardsTarget() {
        transformForward = transform.forward;
        transformRight = transform.right;
        target = Target + transformRight * LaneWidth;
        position = transform.position;
        transformForward.y = 0;

        bool targetBehind = Util.IsInFront(position + transformForward * 2, target, transformForward);
        if (targetBehind) {
            targetDirection = -transformRight;
            forward = transformForward;
        } else {
            targetDirection = target - position;
            targetDirection.y = 0;
            forward = targetDirection;
        }

        Debug.DrawLine(position, position + forward, Color.blue, -1, false);

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

    void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("Road")) return;
        CurrentRoad = col.gameObject.GetComponent<Road>();
        Target = PickTarget();
    }
}
