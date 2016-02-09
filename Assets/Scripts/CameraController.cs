using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject Car;
    public float Distance = 5;
    public float Height = 10;

    private Camera CarCamera;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        CarCamera = GetComponent<Camera>();
	}

    void LateUpdate () {
        Vector3 offset = Car.transform.position - Car.transform.right * Distance;
        Vector3 newPosition = new Vector3(offset.x, Height , offset.z);
        transform.position = newPosition;
        Quaternion rotation = new Quaternion(0, Car.transform.rotation.y, 0, Car.transform.rotation.w);
        transform.rotation = rotation;
        transform.Rotate(0, 90, 0);
    }
}
