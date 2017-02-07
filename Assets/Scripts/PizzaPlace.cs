using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaPlace : MonoBehaviour {
    public Transform MissionTargets;
    public string Tag = "Road";

    void Start () {
        List<Transform> validTargets = new List<Transform>();
        foreach (Transform child in MissionTargets) {
            Collider[] colliders = Physics.OverlapSphere(child.position, 2);
            foreach (Collider col in colliders) {
                if (col.tag == Tag) {
                    validTargets.Add(child);
                    break;
                }
            }
        }

        // pick one of the possible mission targets
        int index = Random.Range(0, validTargets.Count);
        Transform target = validTargets[index];
        target.parent = GameManager.PizzaPlaces.transform;

        // destroy the other targets since the're no longer needed
        Destroy(MissionTargets.gameObject);

        // rotate the building to be facing to the target
        transform.LookAt(target);
    }
}
