using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Car;
    public float Distance = 5;

    private Camera CarCamera;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        CarCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        offset = transform.position - Car.transform.position;
        Debug.Log(Car.transform.position);

    }

    void LateUpdate () {
        transform.position = Car.transform.position + Car.transform.right * Distance;
        transform.rotation = Car.transform.rotation;
    }
}
