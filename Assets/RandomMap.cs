using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RandomMap : MonoBehaviour {
    public TextAsset[] Chunks;
    public GameObject Loader;
    public int SizeX = 10;
    public int SizeZ = 10;
    public float ChunkSize = 9;
    public float TileSize = 4;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                GameObject instance = Instantiate<GameObject>(Loader);

                Vector3 position = new Vector3(x, 0, z) * ChunkSize * TileSize;
                instance.transform.position = position;

                TiledLoader tl = instance.GetComponent<TiledLoader>();
                int chunkIndex = Random.Range(0, Chunks.Length);
                tl.Map = Chunks[chunkIndex];
                tl.Build();

                int angle = Random.Range(0, 3) * 90;
                instance.transform.eulerAngles = Vector3.up * angle;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
