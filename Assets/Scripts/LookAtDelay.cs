using UnityEngine;
using System.Collections;

public class LookAtDelay : MonoBehaviour {
    public float speed = 0.5F;
    public LayerMask Layers; // layers to look at

    void OnTriggerStay(Collider col) {
        if (col.isTrigger || !Util.LayerInLayerMask(Layers, col.gameObject.layer)) return;

        foreach (Transform child in transform) {
            Vector3 lookPos = col.transform.position - child.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            child.rotation = Quaternion.Slerp(child.rotation, rotation, speed * Time.deltaTime);
        }
    }
}
