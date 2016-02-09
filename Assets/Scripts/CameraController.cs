using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Car;

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
        //transform.Translate(transform.position - Car.transform.position);
        Vector3 newPosition = CarCamera.transform.position;
        Debug.Log(Car.transform.position);
        CarCamera.transform.position = Car.transform.position;
    }
}
