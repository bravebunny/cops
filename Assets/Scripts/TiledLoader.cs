using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class TiledLoader : MonoBehaviour {
    public TextAsset Map; // json file generated with Tiled
    public Tile[] Tiles; // index in this array should match the tile index in the tilemap
    public float TileSize = 4; // height/width of the tiles in the scene (in world units)
    public float LayerDepth = 1; // depth difference between Tiled layers
    public Vector3[] LayerScales;
    public bool CombineMeshes = true;
    float CurrentDepth = 0;
    int PreviousDepth = 0;

    [System.Serializable]
    public class Tile {
        public float angle; // rotation of the tile on the y axis
        public GameObject obj; // object to use as tile
    }

    public void Build () {

        Clear(); // delete everything in the map before loading the tiles

        JSONNode json = JSON.Parse(Map.ToString());
        int layerCount = json["layers"].Count;
        int width = json["width"].AsInt;
        int height = json["height"].AsInt;

        for (int layer = 0; layer < layerCount; layer++) {
            GameObject layerObject = new GameObject();
            CombineChildren combine = layerObject.AddComponent<CombineChildren>();
            layerObject.transform.parent = transform;
            JSONArray map = json["layers"][layer]["data"].AsArray;
            string layerName = json["layers"][layer]["name"];
            layerObject.name = "Layer" + layerName;
            int depth = int.Parse(layerName);
            if (depth != PreviousDepth) CurrentDepth += LayerDepth * LayerScales[depth].y;
            Debug.Log("current depth" + depth + ": " + CurrentDepth);
            PreviousDepth = depth;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    int i = x + y * width;

                    int tile = map[i].AsInt - 1;
                    if (tile == -1) continue; // bit of spaghetti

                    GameObject obj = Tiles[tile].obj;
                    if (obj == null) continue; // skip if undefined


                    float angle = Tiles[tile].angle;

                    float objectX = (x - width / 2) * TileSize;
                    float objectY = CurrentDepth;
                    float objectZ = (y - height / 2) * TileSize;

                    Vector3 position = new Vector3(objectX, objectY, objectZ) + transform.position;
                    Quaternion rotation = Quaternion.Euler(0, angle, 0);
                    GameObject instance = (GameObject)Instantiate(obj, position, rotation);

                    instance.transform.parent = layerObject.transform;
                    instance.transform.position = position;
                    // if this is a building, we need to generate all the parts
                    BuildingGenerator bg = instance.GetComponent<BuildingGenerator>();
                    if (bg != null) bg.Generate();

                    Track track = instance.GetComponentInChildren<Track>();
                    if (track != null) track.CreatePillars();

                }
            }
            if (CombineMeshes) combine.Combine();
            layerObject.transform.localScale = LayerScales[depth];
        }
        foreach (Transform child in transform) {
            child.gameObject.isStatic = true;
            child.gameObject.layer = gameObject.layer;
        }
    }

    public void Clear() {
        CurrentDepth = 0;
        PreviousDepth = 0;
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }
}
