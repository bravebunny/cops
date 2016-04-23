using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class HelicopterUserController : MonoBehaviour {
    private HelicopterController Heli; // the car controller we want to use

    private void Awake()
    {
        // get the car controller
        Heli = GetComponent<HelicopterController>();
    }

    // Update is called once per frame
    void Update () {
//        if (!isLocalPlayer) {
//            return;
//        }

        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        float attack = 0f;
        if (Input.GetKey(KeyCode.E))
            attack = 1f;
        else if (Input.GetKey(KeyCode.Q))
            attack = -1f;

        Heli.Move(v, h, attack);
    }
}
