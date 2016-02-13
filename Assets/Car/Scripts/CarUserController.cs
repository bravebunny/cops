using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : NetworkBehaviour {
    private CarController Car; // the car controller we want to use

    private void Awake()
    {
        // get the car controller
        Car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void FixedUpdate () {
//        if (!isLocalPlayer) {
//            return;
//        }

        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        Car.Move(h, v);
    }
}
