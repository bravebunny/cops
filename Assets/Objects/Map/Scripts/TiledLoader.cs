using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class TiledLoader : MonoBehaviour {

    public TextAsset Map; // json file generated with Tiled
    public Tile[] Tiles; // index in this array should match the tile index in the tilemap
    public float TileSize = 4; // height/width of the tiles in the scene (in world units)
    public float LayerHeight = 1; // height difference between Tiled layers

    [System.Serializable]
    public class Tile {
        public float angle; // rotation of the tile on the y axis
        public GameObject obj; // object to use as tile
    }

    public void Build () {

        Clear(); // delete everything in the map before loading the tiles

        JSONNode json = JSON.Parse(Map.ToString());
        JSONArray map = json["layers"][0]["data"].AsArray;
        int size = map.Count;
        int width = json["width"].AsInt;
        int height = json["height"].AsInt;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int i = x + y * width;

                int tile = map[i].AsInt - 1;
                if (tile == -1) continue;

                GameObject obj = Tiles[tile].obj;
                if (obj == null) continue; // skip if undefined
                float angle = Tiles[tile].angle;

                Vector3 position = new Vector3(x * TileSize, 0, y * TileSize);
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                GameObject instance = (GameObject) Instantiate(obj, position, rotation);
                instance.transform.parent = transform;
            }
        }
    }

    public void Clear() {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }
}
