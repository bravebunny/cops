using UnityEngine;
using System.Collections;
using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;

public class CarAIController : MonoBehaviour {

    CarController CarController;    // Reference to actual car controller we are controlling
    Rigidbody Body;
    ExplodeOnImpact ImpactExplosion;
    public Transform Target;               // 'target' the target object to aim for.
    Vector3 TargetPosition;
    Seeker AISeeker;

    public float PathFindingInterval = 1;

    void Awake() {
        CarController = GetComponent<CarController>();
        Body = GetComponent<Rigidbody>();
        ImpactExplosion = GetComponent<ExplodeOnImpact>();
        AISeeker = GetComponent<Seeker>();
    }

    void Start() {
        InvokeRepeating("CalculatePath", Random.Range(0, PathFindingInterval), PathFindingInterval);
        Target = GameManager.Player.transform;
        TargetPosition = Target.position;
    }

    void OnEnable() {
        AISeeker.pathCallback += OnPathComplete;
        // claim?

    }

    public virtual void OnPathComplete(Path path) {
        List<Vector3> vPath = path.vectorPath;
        TargetPosition = vPath[1] - vPath[0];
        //Debug.Log("complete");
    }

    void CalculatePath() {
        AISeeker.StartPath(transform.position, Target.position);
    }

    void FixedUpdate() {
        float steering = Vector3.Dot(transform.right, TargetPosition.normalized);
        CarController.Move(steering, 1);
    }

    public void SetTarget(Transform target) {
        Target = target;
    }

    public void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("FireHydrant")) return;

        ImpactExplosion.Enabled = true;
        CarController.Stabilise = false;
    }

}
