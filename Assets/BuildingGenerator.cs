using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour {
    public GameObject[] Walls;
    public GameObject[] UpperOnlyWalls;
    public GameObject[] GroundOnlyWalls;
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

	public void Generate() {
        int height = Random.Range(1, MaxFloor);

        for (int floor = 0; floor < height; floor++) {
            AddWalls(floor);
        }
        AddCeilling(height);
	}

    void AddObject(GameObject original, Vector3 position, Quaternion rotation) {
        GameObject obj = (GameObject)Instantiate(original, position, rotation);
        obj.transform.parent = transform;
    }

    void AddObject(GameObject original, Vector3 position) {
        AddObject(original, position, Quaternion.identity);
    }

    void AddCeilling(int height) {
        int index = Random.Range(0, Roofs.Length);
        GameObject roof = Roofs[index];
        Vector3 position = transform.position + Vector3.up * height * FloorHeight;
        AddObject(roof, position);
    }

    void AddWalls(int floor) {
        float angle = 0;
        foreach (Vector3 corner in Corners) {
            Vector3 pos = corner * Scale + Vector3.up * floor * FloorHeight;
            AddWall(pos, angle, floor == 0);
            angle += 90;
            AddWall(pos, angle, floor == 0);
        }
    }

    void AddWall(Vector3 pos, float angle, bool groundLevel) {
        List<GameObject> pool = new List<GameObject>(Walls);
        if (groundLevel) pool.AddRange(GroundOnlyWalls);
        else pool.AddRange(UpperOnlyWalls);

        int index = Random.Range(0, pool.Count);
        GameObject wall = pool[index];
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        AddObject(wall, transform.position + pos, rotation);
    }
}
