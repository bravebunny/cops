using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	// How long the object should shake for.
	public float ShakeDuration = 1f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float ShakeAmount = 0.7f;
	public float DecreaseFactor = 1.0f;

    float CurrentTime = 0;
	
	Vector3 OriginalPos;

	void Update() {
		if (CurrentTime > 0) {
            transform.localPosition = transform.localPosition + Random.insideUnitSphere * ShakeAmount;
            CurrentTime -= Time.deltaTime * DecreaseFactor;
		}
	}

    public void Shake() {
        OriginalPos = transform.localPosition;
        CurrentTime = ShakeDuration;
    }
}
