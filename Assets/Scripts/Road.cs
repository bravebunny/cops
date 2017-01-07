using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Road : MonoBehaviour {
    public List<Transform> Connections; // road cells that connect to this one
    public LayerMask RoadLayers;
}
