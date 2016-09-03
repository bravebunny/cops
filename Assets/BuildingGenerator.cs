using UnityEngine;
using System.Collections;

public class BuildingGenerator : MonoBehaviour {
    public GameObject[] Walls;
    public GameObject[] Roofs;
    [Range(1,50)]
    public int MaxFloor = 4;
    public float FloorHeight = 2;
    float Scale = 2;

    Vector3[] Corners = {
        Vector3.left + Vector3.forward,
        Vector3.right + Vector3.forward,
        Vector3.right + Vector3.back,
        Vector3.left + Vector3.back,
    };

	void Start () {
        int height = Random.Range(1, MaxFloor);

        for (int floor = 0; floor < height; floor++) {
            AddWalls(floor);
        }
        AddCeilling(height);
	}

    void AddCeilling(int height) {
        int index = Random.Range(0, Roofs.Length);
        GameObject roof = Roofs[index];
        Vector3 position = transform.position + Vector3.up * height * FloorHeight;
        Instantiate(roof, position, Quaternion.identity);
    }

    void AddWalls(int floor) {
        float angle = 0;
        foreach (Vector3 corner in Corners) {
            Vector3 pos = corner * Scale + Vector3.up * floor * FloorHeight;
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
