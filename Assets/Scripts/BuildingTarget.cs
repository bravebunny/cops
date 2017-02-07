using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTarget : MonoBehaviour {
    void Start() {
        transform.parent = GameManager.Houses.transform;
    }
}
