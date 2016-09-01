using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Road : MonoBehaviour {
    public List<Vector3> Connections; // road cells that connect to this one
    public LayerMask RoadLayers;

    void Start () {
        Connections = new List<Vector3>();
        FindConnection(Vector3.right);
        FindConnection(Vector3.left);
        FindConnection(Vector3.forward);
        FindConnection(Vector3.back);
    }

    void FindConnection(Vector3 direction) {
        RaycastHit hit;
        float slope = 2;
        Vector3 origin = transform.position + Vector3.up * 4;
        Vector3 forward = -transform.up + direction * slope;
        bool ray = Physics.Raycast(origin, forward, out hit, 20, RoadLayers);
        Debug.DrawLine(origin, origin + forward * 5);
        if (ray && (hit.transform.tag == "Road")) {
            Connections.Add(hit.transform.position);
        }
    }
}
