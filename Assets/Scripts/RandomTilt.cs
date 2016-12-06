using UnityEngine;
using System.Collections;

public class RandomTilt : MonoBehaviour {

    public bool X = false;
    public bool Y = false;
    public bool Z = false;

    public int minRotation;
    public int maxRotation;

    // Use this for initialization
    void Start () {
        if (X) transform.Rotate( new Vector3(Random.Range(minRotation, maxRotation), 0, 0));
        if (Y) transform.Rotate(new Vector3(0, Random.Range(minRotation, maxRotation), 0));
        if (Z) transform.Rotate(new Vector3(0, 0, Random.Range(minRotation, maxRotation)));
    }
}
