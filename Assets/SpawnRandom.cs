using UnityEngine;
using System.Collections;

public class SpawnRandom : MonoBehaviour {
    public GameObject[] Objects; // one of these will be spawned
    public Transform[] Positions; // where to spawn objects;

    public float height = 4.5f; // height that must be empty for spawning

    void Start() {
        // check if there is nothing on top of this tile
        Vector3 origin = transform.position + Vector3.up * height;
        bool hit = Physics.Raycast(origin, Vector3.down, height - 0.1f);

        if (!hit) {
            foreach (Transform position in Positions)
                Spawn(position);
        }
        Clear();
    }

	void Spawn(Transform position) {
        int index = Random.Range(0, Objects.Length);
        GameObject instance = Instantiate(Objects[index]);
        instance.transform.position = position.position;
        instance.transform.parent = transform;
        float angle = Random.Range(0, 360);
        instance.transform.eulerAngles = Vector3.up * angle;
	}

    void Clear() {
        // destroy the spawner objects, leaving the spawned ones
        foreach (Transform position in Positions)
            Destroy(position.gameObject);
    }
}
