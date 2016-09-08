using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public GameObject Original; // what will be spawned
    public int Rate = 3; // how many things to spawn

	void Spawn() {
        for (int i = 0; i < Rate; i++) {
            Instantiate(Original, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) Spawn();
    }
}
