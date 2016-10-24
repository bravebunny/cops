using UnityEngine;
using System.Collections;

public class TrainIron : MonoBehaviour {
    public Transform Pivot;
    float InitialLength;

    void Start () {
        InitialLength = Vector3.Distance(transform.position, Pivot.position);
	}
	
	void Update () {
        float scale = Vector3.Distance(transform.position, Pivot.position) / InitialLength;
        transform.localScale = new Vector3(1, 1, scale);

        // vector that points from this object to the pivot
        Vector3 toPivot = Pivot.position - transform.position;
        // horizontal vector used to measure the current angle
        Vector3 horizontal = Vector3.ProjectOnPlane(-transform.forward, Vector3.up);
        float angle = Vector3.Angle(toPivot, horizontal);
        if (toPivot.y < horizontal.y) angle *= -1;
        Vector3 euler = transform.eulerAngles;
        transform.eulerAngles = new Vector3(angle, euler.y, euler.z);
    }
}
