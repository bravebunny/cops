using UnityEngine;
using System.Collections;

public class CloudGenerator : MonoBehaviour {
    public GameObject CloudPart; // object that will be instantiated to give shape to the cloud
    public int NumberOfParts = 10; // how many parts to generate
    public float ScaleRandomness = 0.5f;
    
	void Start () {
        float radius = GetComponent<Renderer>().bounds.extents.x;
        Debug.Log("Radius: " + radius);
        for (int i = 0; i < NumberOfParts; i++) {
            // create the new cloud part
            GameObject part = Instantiate<GameObject>(CloudPart);

            // generate a random number to slightly vary the scale of each part
            float randomness = 1 + Random.Range(-ScaleRandomness, ScaleRandomness);

            // position the new part around the center of the cloud
            float angle = (360 / NumberOfParts) * i;
            part.transform.parent = transform;
            part.transform.position = transform.position + Vector3.forward * radius * randomness;
            part.transform.RotateAround(transform.position, Vector3.up, angle);

            // change the scale
            part.transform.localScale = Vector3.one * randomness;
        }
	}
}
