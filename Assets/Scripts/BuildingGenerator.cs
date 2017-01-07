using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : RunOnMapBuild {
    public GameObject[] Walls;
    public GameObject[] UpperOnlyWalls;
    public GameObject[] GroundOnlyWalls;
    public GameObject[] Roofs;
    public GameObject PropGroup;
    [Range(1,50)]
    public int MaxFloor = 4;
    public float FloorHeight = 2;
    public float MaxDistanceFromCenter = 240f;
    float Scale = 2;

    Vector3[] Corners = {
        Vector3.left + Vector3.forward,
        Vector3.right + Vector3.forward,
        Vector3.right + Vector3.back,
        Vector3.left + Vector3.back,
    };

	public override void Execute() {
        // distance from center affects building density
        float distanceFromCenter = 1 - Vector3.Distance(transform.position, Vector3.zero) / MaxDistanceFromCenter;
        //Debug.Log(distanceFromCenter);
        float height = Random.Range(0, MaxFloor) * distanceFromCenter;
        if (height < 0.5f) {
            CreatePropGroup();
            return;
        }

        for (int floor = 0; floor < height; floor++) {
            AddWalls(floor);
        }
        
        AddCeilling((int) Mathf.Ceil(height));
        GetComponent<CombineChildren>().Combine();
        foreach (Transform child in transform) {
            child.gameObject.isStatic = false;
        }

        // change the collider to fit the generated building
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 size = collider.size;
        size.y *= Mathf.Ceil(height);
        collider.size = size;
        Vector3 center = collider.center;
        center.y = size.y / 2;
        collider.center = center;
    }

    void CreatePropGroup() {
        GameObject instance = Instantiate<GameObject>(PropGroup);
        instance.transform.position = transform.position;
        instance.transform.parent = transform;
        DestroyImmediate(GetComponent<BoxCollider>());
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
