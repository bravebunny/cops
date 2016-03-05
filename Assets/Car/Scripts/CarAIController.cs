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
    [SerializeField] private Transform Target;                                              // 'target' the target object to aim for.
    [SerializeField] private bool StopWhenTargetReached;                                    // should we stop driving when we reach the target?
    [SerializeField] private float ReachTargetThreshold = 2;                                // proximity to target to consider we 'reached' it, and stop driving.

    private float RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    private CarController CarController;    // Reference to actual car controller we are controlling
    private float AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
    private float AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
    private float AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
    private Rigidbody Rigidbody;

    private void Awake()
    {
        // get the car controller
        CarController = GetComponent<CarController>();
        // Allows to disable camera collisions with cops
        CarController.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
        // give the random perlin a random value
        RandomPerlin = Random.value*100;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Target == null) {
            CarController.Move(0, 0);
            return;
        }

        NavMeshPath path = new NavMeshPath();

        if (CarController.Blocked) {
            //NavMesh.CalculatePath(transform.position, transform.position - transform.forward * 5, -1, path);
            CarController.Move(5, 0);
        } else {
            NavMesh.CalculatePath(transform.position, Target.position, -1, path);
        }
        int pathElements = path.corners.Length;

        // Draw the path
        for(int i=1;i<pathElements;++i) {
            Debug.DrawLine(path.corners[i-1], path.corners[i], Color.green);
        }

        // our target position starts off as the 'real' target position
        Vector3 offsetTargetPos = Target.position;
        if (pathElements >= 2) {
            offsetTargetPos = path.corners[1];
        }

       
        // no need for evasive action, we can just wander across the path-to-target in a random way,
        // which can help prevent AI from seeming too uniform and robotic in their driving
        offsetTargetPos += Target.forward*
            (Mathf.PerlinNoise(Time.time*LateralWanderSpeed, RandomPerlin)*2 - 1)*
            LateralWanderDistance;

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
        Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

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
}
