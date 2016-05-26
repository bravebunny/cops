using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TiledLoader : MonoBehaviour {

    public TextAsset Map; // json file generated with Tiled
    public Tile[] Tiles; // index in this array should match the tile index in the tilemap
    public float TileSize = 4; // height/width of the tiles in the scene

    private int height;
    private int width;

    [System.Serializable]
    public class Tile {
        public float angle; // rotation of the tile on the y axis
        public GameObject obj; // object to use as tile
    }

    void Start () {

        JSONNode json = JSON.Parse(Map.ToString());
        JSONArray map = json["layers"][0]["data"].AsArray;
        int size = map.Count;
        int width = json["width"].AsInt;
        int height = json["height"].AsInt;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int i = x + y * width;

                int tile = map[i].AsInt - 1;
                Debug.Log("tile: " + tile);
                GameObject obj = Tiles[tile].obj;
                if (obj == null) continue; // skip if undefined
                float angle = Tiles[tile].angle;

                Vector3 position = new Vector3(x * TileSize, 0, y * TileSize);
                Quaternion rotation = new Quaternion(0, angle, 0, 0);
                Instantiate(obj, position, rotation);
            }
        }

        /*JSONObject j = new JSONObject(Map.ToString());
        accessData(j);*/
    }

    //access data (and print it)
    void accessData(JSONObject obj) {
        switch (obj.type) {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++) {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list) {
                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;
        }
    }
}
