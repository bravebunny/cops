using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RandomMap : MonoBehaviour {
    public Chunk[] BaseChunks;
    public Chunk[] Chunks;
    public Chunk Empty;
    public GameObject Loader;
    public int SizeX = 10;
    public int SizeZ = 10;
    public float ChunkSize = 9;
    public float TileSize = 4;
    Voxel[,] Voxels;
    float Seed;

    void Start () {
        Seed = Random.value;

        Chunks = new Chunk[BaseChunks.Length * 4];

        for (int i = 0; i < BaseChunks.Length; i++) {
            for (int rotation = 0; rotation < 4; rotation++) {
                Chunk newChunk = new Chunk(BaseChunks[i]);
                newChunk.SetRotation(rotation);
                Chunks[i * 4 + rotation] = newChunk;
            }
        }

        Voxels = new Voxel[SizeX, SizeZ];
        float size = ChunkSize * TileSize;
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                Voxels[x, z] = new Voxel(x, z, size);
            }
        }

        PopulateVoxels();

        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                CreateChunk(x, z);
            }
        }
    }

    void CreateChunk(int x, int z) {
        Voxel current = Voxels[x, z];
        if (!current.value) {
            InstantiateChunk(Empty, current.Position);
            return;
        }

        bool top = false, right = false, bottom = false, left = false;
        if (z + 1 < Voxels.GetLength(1)) top = Voxels[x, z + 1].value;
        if (x + 1 < Voxels.GetLength(0)) right = Voxels[x + 1, z].value;
        if (z > 0) bottom = Voxels[x, z - 1].value;
        if (x > 0) left = Voxels[x - 1, z].value;
        
        List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in Chunks) {
            if (chunk.top == top &&
                chunk.right == right &&
                chunk.bottom == bottom &&
                chunk.left == left) {
                InstantiateChunk(chunk, current.Position);
                return;
            }
        }
    }

    /*
    Chunk PickChunkX(Voxel current, Voxel x) {
        List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in Chunks) {
            if (chunk.left == current.top && chunk.bottom == x.left)
                validChunks.Add(chunk);
        }
        int index = Random.Range(0, validChunks.Count);
        return validChunks[index];
    }*/

    void InstantiateChunk(Chunk chunk, Vector3 position) {
        GameObject instance = Instantiate<GameObject>(Loader);
        instance.transform.position = position;
        TiledLoader tl = instance.GetComponent<TiledLoader>();
        tl.Map = chunk.TiledMap;
        tl.Build();
        tl.transform.eulerAngles = Vector3.up * chunk.rotation * 90;
    }

    [System.Serializable]
    public class Chunk {
        public TextAsset TiledMap;
        public bool top;
        public bool right;
        public bool bottom;
        public bool left;
        public int rotation;

        public Chunk(Chunk chunk) {
            TiledMap = chunk.TiledMap;
            top = chunk.top;
            right = chunk.right;
            bottom = chunk.bottom;
            left = chunk.left;
        }

        public void SetRotation(int rotation) {
            for (int i = 0; i < rotation; i++) RotateRight();
        }

        public void RotateRight() {
            //Debug.Log("Before rotation: " + ToString());
            bool oldTop = top;
            top = left;
            left = bottom;
            bottom = right;
            right = oldTop;
            rotation++;
            //Debug.Log("After rotation: " + ToString());
        }

        public string ToString() {
            return top + "," + right + "," + bottom + "," + left;
        }
    }

    public class Voxel {
        public bool value = false;
        public Vector3 Position;
        public Vector3 ZCorner;
        public Vector3 XCorner;

        public Voxel (int x, int z, float size) {
            Position = new Vector3(x * size, 0, z * size);
            ZCorner = Position + Vector3.forward * size;
            XCorner = Position + Vector3.right * size;
        }
    }

    int MinLenght = 2;
    int MaxLength = 5;
    int Size = 500;
    float StepSize = 1;

    int xDiff = 0, yDiff = 0;
    int currentX = 0, currentY = 0;

    void PopulateVoxels() {
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                //Debug.Log("perlin: " + perlin);
                if (perlin > 0.35) Voxels[x, z].value = true;
            }
        }
    }

    void OnDrawGizmos() {
        if (Voxels == null) return;
        foreach (Voxel v in Voxels) {
            Gizmos.color = v.value ? Color.blue : Color.black;
            Gizmos.DrawCube(v.Position, Vector3.one * 3);
        }
    }
}
