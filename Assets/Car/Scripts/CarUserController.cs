using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CarUserController : MonoBehaviour {
    private CarController Car; // the car controller we want to use

    private void Awake()
    {
        // get the car controller
        Car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        Car.Move(h, v);
    }
}
