using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour {
    public GameObject[] Objects; // objects to be generated
    public int GridSize = 10;
    public float CellSize = 5;

    void Awake () {
        float xz = -(GridSize * CellSize) / 2;
        Vector3 corner = new Vector3(xz, 0, xz);
        Vector3 extents = Vector3.one * CellSize / 2;
        Vector3 center;
        for (int x = 0; x < GridSize; x++) {
            for (int z = 0; z < GridSize; z++) {
                center = corner + new Vector3(x * CellSize, 0, z * CellSize);
                Debug.DrawLine(center, center + Vector3.right * CellSize, Color.cyan, -1, false);
                Debug.DrawLine(center, center + Vector3.forward * CellSize, Color.cyan, -1, false);
                Collider[] colliders = Physics.OverlapBox(center, extents);
                List<BuildingGenerator> buildings = new List<BuildingGenerator>();
                foreach (Collider col in colliders) {
                    BuildingGenerator bg = col.GetComponent<BuildingGenerator>();
                    if (bg == null || buildings.Contains(bg)) continue;
                    buildings.Add(bg);
                }
                int count = buildings.Count;
                if (count == 0) continue;
                foreach (GameObject obj in Objects) {
                    Vector3 position = PickPosition(buildings);
                    Instantiate(obj, position, Quaternion.identity);
                }
            }
        }
	}

    Vector3 PickPosition(List<BuildingGenerator> buildings) {
        int count = buildings.Count;
        int index = Random.Range(0, count);
        if (buildings[index].gameObject == null) return PickPosition(buildings);
        Vector3 position = buildings[index].transform.position;
        Destroy(buildings[index].gameObject);
        return position;
    }
}
