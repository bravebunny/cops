using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float SmoothTime = 0.3F;
    public float Distance = 5.0F;
    public float Height = 10;

    public LayerMask collisionLayer;
    public bool cameraBack = true;
    public bool debugDesiredCam;
    public bool debugAdjustedCam;

    private new Camera camera;
    private bool colliding = false;
    private Vector3[] adjustedCameraClipPoints = new Vector3[5];
    private Vector3[] desiredCameraClipPoints = new Vector3[5];
    private Vector3 position = Vector3.zero;
    private Vector3 ajustedDestination = Vector3.zero;
    private Vector3 camVel = Vector3.zero;
    private float adjustmentDistance = -10;
    private double carDirection = 0;

    void Start() {
        camera = GetComponentInChildren<Camera>();
        UpdateCameraClipPoints(transform.position, transform.rotation, ref adjustedCameraClipPoints);
        UpdateCameraClipPoints(position, transform.rotation, ref desiredCameraClipPoints);
    }

    void FixedUpdate() {
        if (target == null) {
            return;
        }

        carDirection = target.GetComponent<CarController>().carMovementDirection;
        carDirection = System.Math.Round(carDirection * 10) / 10;

        if (cameraBack) {
            if (carDirection >= 0) {
                carDirection = 1;
            }
            else {
                carDirection = System.Math.Max(1, target.GetComponent<CarController>().carMovementDirection * -0.1);
            }
        } else {
            if (carDirection < 0) {
                carDirection = 1;
            }
            else {
                carDirection = System.Math.Max(1, target.GetComponent<CarController>().carMovementDirection * 0.1);
            }
        }

        UpdateCameraClipPoints(transform.position, transform.rotation, ref adjustedCameraClipPoints);
        UpdateCameraClipPoints(position, transform.rotation, ref desiredCameraClipPoints);

        //draw debug lines
        for (int i = 0; i < 5; i++)
        {
            if (debugDesiredCam) Debug.DrawLine(target.position, desiredCameraClipPoints[i], Color.red);
            if (debugAdjustedCam) Debug.DrawLine(target.position, adjustedCameraClipPoints[i], Color.green);
        }

        CheckColliding(target.position); //fixedUpdate because it uses rayCasts
        adjustmentDistance = GetAdjustedDistancewithRayFrom(target.position);

        position = target.position;
        if (cameraBack)
            position += Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0) * new Vector3(0, Height, -Distance * (float) carDirection);
        else
            position += Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0) * new Vector3(0, Height, -Distance * (float)-carDirection);

        if (colliding) {
            ajustedDestination = target.position;
            ajustedDestination += Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0) * new Vector3(0, Height + 2, -adjustmentDistance);
            transform.position = Vector3.SmoothDamp(transform.position, ajustedDestination, ref camVel, SmoothTime);
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, position, ref camVel, SmoothTime);
        }
        transform.LookAt(target);
    }

    public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) {
        intoArray = new Vector3[5];
        float z = camera.nearClipPlane;
        float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
        float y = x / camera.aspect;

        //top left
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
        //top right
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
        //bottom left
        intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
        //bottom right
        intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;
        //camera position
        intoArray[4] = cameraPosition - camera.transform.forward;
    }

    bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            float distance = Vector3.Distance(clipPoints[i], fromPosition);
            if (Physics.Raycast(ray, distance, collisionLayer)) return true;
        }
        return false;
    }

    public float GetAdjustedDistancewithRayFrom(Vector3 from) {
        float distance = -1;

        for (int i = 0; i < desiredCameraClipPoints.Length; i++) {
            Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (distance == -1) distance = hit.distance;
                else {
                    if (hit.distance < distance) distance = hit.distance;
                }
            }
        }

        if (distance == -1) return 0;
        else return distance;
    }

    public void CheckColliding(Vector3 targetPosition) {
        if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition)) colliding = true;
        else colliding = false;
    }
}

