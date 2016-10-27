using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public GameObject Original; // what will be spawned
    public int Rate = 3; // how many things to spawn
    public float Interval = 0.1f; // seconds between spawns

	void SpawnAll() {
        for (int i = 0; i < Rate; i++) {
            Invoke("Spawn", Interval * i);
        }
    }

    void Spawn() {
        GameManager.CopCount++;
        Instantiate(Original, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) SpawnAll();
    }
}
