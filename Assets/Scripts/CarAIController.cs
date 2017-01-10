using UnityEngine;

public class CarAIController : MonoBehaviour {
    ExplodeOnImpact ImpactExplosion;
    public Transform Target;               // 'target' the target object to aim for.
    public float LookDistance = 5; // distance to look ahead for obstacles
    public float AvoidDistance = 1; // distance to add to the new direction to avoid obstacles
    public float Speed = 100;
    public float UpdateRate = 0.5f; // interval in seconds to update direction
    public LayerMask ObstacleLayers;
    Vector3 Direction;
    Vector3 Velocity;
    Rigidbody Body;

    void Awake() {
        ImpactExplosion = GetComponent<ExplodeOnImpact>();
        Body = GetComponent<Rigidbody>();
    }

    void Start() {
        GameManager.CopCount++;
        Target = GameManager.Player.transform;
        InvokeRepeating("UpdateDirection", Random.Range(0, UpdateRate), UpdateRate);
    }

    void UpdateDirection() {
        transform.LookAt(Target);
        Velocity = transform.forward * Speed;
        Velocity.y = 0;
    }

    void FixedUpdate() {
        //Direction = (Target.position - transform.position).normalized;
        Body.AddForce(Velocity);

        /*RaycastHit info;
        bool hit = Physics.Raycast(transform.position, Direction, out info, LookDistance, ObstacleLayers);
        Debug.DrawLine(transform.position, transform.position + Direction * LookDistance, Color.blue, -1, false);
        if (hit) {
            Direction += Vector3.ProjectOnPlane(transform.right, Vector3.up) * AngleDir(transform.forward, info.normal, Vector3.up);
            Direction.Normalize();
            Debug.DrawLine(transform.position, transform.position + Direction, Color.red, -1, false);
        }*/

        //CarController.Move(Vector3.Dot(transform.right, Direction), 1);
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f) {
            return 1f;
        } else {
            return -1f;
        }
    }

    void OnDestroy() {
        GameManager.CopCount--;
        GameManager.KilledCops++;
    }
}
