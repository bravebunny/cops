﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
    public float Speed = 5;
    public float MaxGroundDistance = 0.5f;
    public float RotationSpeed = 150;
    public float LaneWidth = 2.5f;
    Rigidbody Body;
    Road CurrentRoad;
    bool Grounded;
    [HideInInspector] public Vector3 Target;

	void Start() {
        Body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate() {
        Road road = GetCurrentRoad();
        if (road != CurrentRoad && road != null) {
            CurrentRoad = road;
            Target = PickTarget();
        }
        if (Grounded) MoveTowardsTarget();
	}

    void MoveTowardsTarget() {
        Vector3 target = Target + transform.right * LaneWidth;
        Vector3 targetDirection;
        Vector3 forward;

        bool targetBehind = Util.IsInFront(transform.position + transform.forward * 2, target, transform.forward);
        if (targetBehind) {
            forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            targetDirection = -transform.right;
        } else {
            targetDirection = Vector3.ProjectOnPlane((target - transform.position), Vector3.up);
            forward = targetDirection;
        }
        Body.AddForce(forward.normalized * Speed, ForceMode.VelocityChange);
        Body.angularVelocity = Vector3.zero;
        Body.AddTorque(Vector3.Cross(Vector3.ProjectOnPlane(transform.forward, Vector3.up), targetDirection) * RotationSpeed, ForceMode.Acceleration);
    }

    // pick one of the connections randomly
    Vector3 PickTarget() {
        List<Vector3> connections = CurrentRoad.Connections;
        if (connections.Count == 1) return CurrentRoad.Connections[0];

        List<Vector3> targets = new List<Vector3>();
        foreach (Vector3 connection in CurrentRoad.Connections) {
            bool inFront = Util.IsInFront(connection, transform.position - transform.forward * 2, transform.forward);
            if (inFront) targets.Add(connection);
        }
        int index = Random.Range(0, targets.Count);
        return targets[index];
    }

    // get the road that's currently under the car
    Road GetCurrentRoad() {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = -transform.up;
        Grounded = Physics.Raycast(origin, direction, out hit, MaxGroundDistance);
        Debug.DrawLine(origin, origin + direction * MaxGroundDistance);
        if (!Grounded) return null;
        return hit.transform.GetComponent<Road>();
    }
}
