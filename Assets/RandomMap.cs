using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RandomMap : MonoBehaviour {
    public Chunk[] Chunks;
    public GameObject Loader;
    public int SizeX = 10;
    public int SizeZ = 10;
    public float ChunkSize = 9;
    public float TileSize = 4;

	void Start () {
        Debug.Log("## first ##");
        Chunk first = PickFirstChunk();
        Debug.Log("## next ##");
        Chunk previous = first;
        for (int x = 0; x < 5; x++) {
            previous = PickNextChunk(previous);
        }
        /*for (int x = 0; x < SizeX; x++) {
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
        }*/
    }

    Chunk RandomChunk() {
        int index = Random.Range(0, Chunks.Length);
        Chunk chunk = new Chunk(Chunks[index]);
        chunk.RandomRotation();
        return chunk;
    }

    Chunk PickFirstChunk() {
        Chunk chunk = RandomChunk();

        GameObject instance = Instantiate<GameObject>(Loader);
        TiledLoader tl = instance.GetComponent<TiledLoader>();
        tl.Map = chunk.TiledMap;
        tl.Build();

        int angle = chunk.rotation * 90;
        instance.transform.eulerAngles = Vector3.up * angle;
        Debug.Log(chunk.ToString());
        return chunk;
    }

    void InstantiateChunk(Chunk chunk, Vector3 euler) {
        GameObject instance = Instantiate<GameObject>(Loader);
        TiledLoader tl = instance.GetComponent<TiledLoader>();
        tl.Map = chunk.TiledMap;
        tl.Build();
        instance.transform.position = chunk.position;
        instance.transform.eulerAngles = euler;
    }

    Chunk PickNextChunk(Chunk previous) {
        Chunk chunk = RandomChunk();
        int rotationCount = 0;
        while (previous.right != chunk.left) {
            chunk.RotateRight();
            Debug.Log("$while" + rotationCount + ": previous(" + previous.ToString() + ") current(" + chunk.ToString() + ")");
            rotationCount++;
            if (rotationCount > 4) {
                Debug.LogError("No possible chunk found");
                Debug.Log(chunk.ToString());
                break;
            }
        }
        Debug.Log("$$ after while: previous=" + previous.right + ", current=" + chunk.left);
        Vector3 euler = Vector3.up * chunk.rotation * 90;
        chunk.position = previous.position + Vector3.right * ChunkSize * TileSize;
        InstantiateChunk(chunk, euler);
        Debug.Log(chunk.ToString());
        return chunk;
    }

    [System.Serializable]
    public class Chunk {
        public TextAsset TiledMap;
        public bool top;
        public bool right;
        public bool bottom;
        public bool left;
        public int rotation;
        public Vector3 position;

        public Chunk(Chunk chunk) {
            TiledMap = chunk.TiledMap;
            top = chunk.top;
            right = chunk.right;
            bottom = chunk.bottom;
            left = chunk.left;
            rotation = chunk.rotation;
        }

        public void RandomRotation() {
            int amount = Random.Range(0, 4);
            for (int i = 0; i < amount; i++) {
                RotateRight();
            }
        }

        public void RotateRight() {
            Debug.Log("Before rotation: " + ToString());
            bool oldTop = top;
            top = left;
            left = bottom;
            bottom = right;
            right = oldTop;
            rotation--;
            Debug.Log("After rotation: " + ToString());
        }

        public string ToString() {
            return top + "," + right + "," + bottom + "," + left;
        }
    }
}
