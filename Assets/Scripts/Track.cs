using UnityEngine;
using System.Collections;

public class Track : MonoBehaviour {
    public Transform A;
    public Transform B;
    public bool CheckIfBridge;
    public GameObject Pillars;
    float MaxHeight = 0.1f;
    float HeightOffset = 0.0005f;

    public void CreatePillars() {
        if (!CheckIfBridge) return;
        RaycastHit info;
        bool hit = Physics.Raycast(transform.position + Vector3.up * HeightOffset, Vector3.down, out info);
        if (info.distance > MaxHeight || !hit) {
            GameObject pillar = (GameObject)Instantiate(Pillars, transform.position, transform.rotation);
            pillar.transform.parent = transform.parent;
            pillar.isStatic = true;
        }
    }
}
