using UnityEngine;
using System.Collections;

public class BuildingGenerator : MonoBehaviour {
    public GameObject[] Walls;
    public float Size;


	void Start () {
        Vector3[] positions = {
            Vector3.left + Vector3.forward,
            Vector3.right + Vector3.forward,
            Vector3.right + Vector3.back,
            Vector3.left + Vector3.back,
        };

        float angle = 0;
        foreach (Vector3 pos in positions) {
            AddWall(pos, angle);
            angle += 90;
            AddWall(pos, angle);
        }
	}

    void AddWall(Vector3 pos, float angle) {
        int index = Random.Range(0, Walls.Length);
        GameObject wall = Walls[index];
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        GameObject obj = (GameObject) Instantiate(wall, transform.position + pos, rotation);
        obj.transform.parent = transform;
    }
}
