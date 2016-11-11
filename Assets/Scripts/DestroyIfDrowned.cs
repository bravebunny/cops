using UnityEngine;
using System.Collections;

public class DestroyIfDrowned : MonoBehaviour {
    public float MinY = -10;
	
	void Update () {
        if (transform.position.y < MinY) Destroy(gameObject);
	}
}
