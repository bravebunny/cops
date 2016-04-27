using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour {

    // Each color is mapped to the object in the same position
    public Tile[] tiles;

    public Texture2D texture;
    public float tileSize = 4;

    void Start() {
        LoadMap();
	}
	
	void Update() {
	
	}

    void LoadMap() {
        // Pair up objects and colors in a dictionary
        Dictionary<Color, GameObject> tileDic = new Dictionary<Color, GameObject>();
        for (int i = 0; i < tiles.Length; i++) {
            tileDic.Add(tiles[i].color, tiles[i].obj);
        }

        // Instantiate objects from pixels 
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                Color c = texture.GetPixel(x, y);
                Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
                Quaternion rotation = new Quaternion(0, 0, 0, 0);
                Instantiate(tileDic[c], position, rotation);
            }
        }
        
    }
}

[System.Serializable]
public class Tile {
    public Color color;
    public GameObject obj;
}
