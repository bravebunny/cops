using UnityEngine;

public class CarAIController : MonoBehaviour {

    CarController CarController;    // Reference to actual car controller we are controlling
    ExplodeOnImpact ImpactExplosion;
    public Transform Target;               // 'target' the target object to aim for.
    Vector3 Direction;

    void Awake() {
        CarController = GetComponent<CarController>();
        ImpactExplosion = GetComponent<ExplodeOnImpact>();
    }

    void Start() {
        GameManager.CopCount++;
        Target = GameManager.Player.transform;
    }

    void FixedUpdate() {
        Direction = (Target.position - transform.position).normalized;
        CarController.Move(Vector3.Dot(transform.right, Direction), 1);
    }

    public void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("FireHydrant")) return;
        CarController.Stabilise = false;
        // TODO move this away from here
    }

    void OnDestroy() {
        GameManager.CopCount--;
        GameManager.KilledCops++;
    }
}
