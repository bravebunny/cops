using UnityEngine;
using System.Collections;

public class PoliceLights : MonoBehaviour {
    public Light Red;
    public Light Blue;
    public float Rate = 1;

	void Start () {
        Red.enabled = false;
        Blue.enabled = true;

        // Wait a random amount of time before starting,
        // so all the cops' lights don't blink at the same time
        float wait = Random.Range(0, Rate);
        InvokeRepeating("Switch", wait, Rate);
	}
	
	void Switch() {
        Red.enabled = !Red.enabled;
        Blue.enabled = !Blue.enabled;
    }
}
