using UnityEngine;
using System.Collections;

public class CarAIController : MonoBehaviour {
    // "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
    // in speed and direction while driving towards their target.

    [SerializeField] [Range(0, 1)] private float CautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
    [SerializeField] [Range(0, 180)] private float CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
    [SerializeField] private float CautiousMaxDistance = 100f;                              // distance at which distance-based cautiousness begins
    [SerializeField] private float CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
    [SerializeField] private float SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
    [SerializeField] private float AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
    [SerializeField] private float BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
    [SerializeField] private float LateralWanderDistance = 3f;                              // how far the car will wander laterally towards its target
    [SerializeField] private float LateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
    [SerializeField] [Range(0, 1)] private float AccelWanderAmount = 0.1f;                  // how much the cars acceleration will wander
    [SerializeField] private float AccelWanderSpeed = 0.1f;                                 // how fast the cars acceleration wandering will fluctuate
    [SerializeField] private bool Driving;                                                  // whether the AI is currently actively driving or stopped.
    [SerializeField] private bool StopWhenTargetReached;                                    // should we stop driving when we reach the target?
    [SerializeField] private float ReachTargetThreshold = 2;                                // proximity to target to consider we 'reached' it, and stop driving.

    float RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    CarController CarController;    // Reference to actual car controller we are controlling
    float AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
    float AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
    float AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
    Rigidbody Body;
    ExplodeOnImpact ImpactExplosion;
    Transform Target;                                                               // 'target' the target object to aim for.
    Vector3 TargetPosition;
    AIPath Path;

    public float PathFindingInterval = 1;

    void Awake() {
        Path = GetComponent<AIPath>();
        CarController = GetComponent<CarController>();
        // Allows to disable camera collisions with cops
        CarController.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        // give the random perlin a random value
        RandomPerlin = Random.value*100;
        Body = GetComponent<Rigidbody>();
        ImpactExplosion = GetComponent<ExplodeOnImpact>();
    }

    void Start() {
        InvokeRepeating("CalculatePath", Random.Range(0, PathFindingInterval), PathFindingInterval);
        Target = GameManager.Player.transform;
        TargetPosition = Target.position;
    }

    void CalculatePath() {
        if (CarController.Blocked) return;
        TargetPosition = Path.lastFoundWaypointPosition;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Target == null) {
            CarController.Move(0, 0);
            return;
        }

        if (CarController.Blocked) {
            //NavMesh.CalculatePath(transform.position, transform.position - transform.forward * 5, -1, path);
            CarController.Move(5, 0);
        }

        float desiredSpeed = 100;

        // use different sensitivity depending on whether accelerating or braking:
        float accelBrakeSensitivity = (desiredSpeed < CarController.CurrentSpeed)
            ? BrakeSensitivity
            : AccelSensitivity;

        // decide the actual amount of accel/brake input to achieve desired speed.
        float accel = Mathf.Clamp((desiredSpeed - CarController.CurrentSpeed)*accelBrakeSensitivity, -1, 1);

        // add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving
        // i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
        accel *= (1 - AccelWanderAmount) +
            (Mathf.PerlinNoise(Time.time*AccelWanderSpeed, RandomPerlin)*AccelWanderAmount);

        // calculate the local-relative position of the target, to steer towards
        Vector3 localTarget = transform.InverseTransformPoint(TargetPosition);

        // work out the local angle towards the target
        float targetAngle = -Mathf.Atan2(localTarget.z, localTarget.x)*Mathf.Rad2Deg + 90;

        // get the amount of steering needed to aim the car towards the target
        float steer = Mathf.Clamp(targetAngle*SteerSensitivity, -1, 1)*Mathf.Sign(CarController.CurrentSpeed);

        // feed input to the car controller.
        CarController.Move(steer, accel);

        // if appropriate, stop driving when we're close enough to the target.
        if (StopWhenTargetReached && localTarget.magnitude < ReachTargetThreshold)
        {
            Driving = false;
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("FireHydrant")) return;

        ImpactExplosion.Enabled = true;
        CarController.Stabilise = false;
    }
}
