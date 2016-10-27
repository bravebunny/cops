using UnityEngine;
using System.Collections;

public class DestroyRandom : MonoBehaviour {

    public float Probability = 0.5F;

    // Use this for initialization
    void Start() {
        if (Random.Range(0.0F, 1.0F) <= Probability) Destroy(gameObject);
    }
}