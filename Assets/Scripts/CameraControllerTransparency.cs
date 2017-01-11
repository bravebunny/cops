using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTransparency : MonoBehaviour {

    public Transform target;
    public float SmoothTime = 0.3F;
    public float Distance = 5.0F;
    public float Height = 10;

    public bool RearView = false;

    private Vector3 position = Vector3.zero;
    private Vector3 camVel = Vector3.zero;
    private double carDirection = 0;

    void FixedUpdate() {
        if (target == null) {
            return;
        }

        carDirection = target.GetComponent<CarController>().carMovementDirection;
        carDirection = System.Math.Round(carDirection * 10) / 10;

        if (!RearView) {
            if (carDirection >= 0) {
                carDirection = 1;
            } else {
                carDirection = System.Math.Max(1, target.GetComponent<CarController>().carMovementDirection * -0.05);
            }
        } else {
            if (carDirection < 0) {
                carDirection = 1;
            } else {
                carDirection = System.Math.Max(1, target.GetComponent<CarController>().carMovementDirection * 0.1);
            }
        }

        position = target.position;
        if (!RearView)
            position += Quaternion.Euler(0, target.eulerAngles.y, 0) * new Vector3(0, Height, -Distance * (float)carDirection);
        else
            position += Quaternion.Euler(0, target.eulerAngles.y, 0) * new Vector3(0, Height, -Distance * (float)-carDirection);

        
        transform.position = Vector3.SmoothDamp(transform.position, position, ref camVel, SmoothTime);
        transform.LookAt(target);
    }
}
