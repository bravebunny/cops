using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RandomMap : MonoBehaviour {
    public TextAsset[] BaseChunks;
    public Chunk[] Chunks;
    public Chunk Empty;
    public GameObject Loader;
    public int SizeX = 10;
    public int SizeZ = 10;
    public float ChunkSize = 9;
    public float TileSize = 4;

    public float CityRadius = 5;
    public float CityMinPerlin = 0.35f;
    public float IslandRadius = 0.0001f;
    public float IslandMinPerlin = 0.05f;

    Voxel[,] Voxels;
    float Seed;
    int SeedRange = 10000;
    float SeedOffset = 0.5f;


    public class Voxel {
        public int value = 1;
        public Vector3 Position;
        public Vector3 ZCorner;
        public Vector3 XCorner;

        public Voxel(int x, int z, float size) {
            Position = new Vector3(x * size, 0, z * size);
            ZCorner = Position + Vector3.forward * size;
            XCorner = Position + Vector3.right * size;
        }
    }


    [System.Serializable]
    public class Chunk {
        public TextAsset TiledMap;
        public int top;
        public int right;
        public int bottom;
        public int left;
        public int rotation;

        public Chunk(Chunk chunk) {
            TiledMap = chunk.TiledMap;
            top = chunk.top;
            right = chunk.right;
            bottom = chunk.bottom;
            left = chunk.left;
        }

        public Chunk(TextAsset tiledMap) {
            TiledMap = tiledMap;
            // get the values of this chunk from the file name
            char[] chars = tiledMap.name.ToCharArray();
            int length = chars.Length;
            int offset = 48; // used to convert from char to int
            left = chars[length - 1] - offset;
            bottom = chars[length - 2] - offset;
            right = chars[length - 3] - offset;
            top = chars[length - 4] - offset;
            //Debug.Log(ToString());
        }

        public void SetRotation(int rotation) {
            for (int i = 0; i < rotation; i++) Rotate();
        }

        public void Rotate() {
            //Debug.Log("Before rotation: " + ToString());
            int oldTop = top;
            top = left;
            left = bottom;
            bottom = right;
            right = oldTop;
            rotation++;
            //Debug.Log("After rotation: " + ToString());
        }

        public override string ToString() {
            return TiledMap.name + ": " + top + "," + right + "," + bottom + "," + left;
        }
    }

    void Start () {
        // create a large integer number for the perlin noise seed
        // the offset isn't random because it affects the density of the noise
        Seed = Random.Range(1, SeedRange) + SeedOffset;

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
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                distance *= distance * distance * distance;
                // create chunks only inside the island
                //if (perlin > IslandMinPerlin + IslandRadius * distance) CreateChunk(x, z);

                CreateChunk(x, z);
            }
        }
    }

    void CreateChunk(int x, int z) {

        Voxel current = Voxels[x, z];
        if (current.value == 1) {
            InstantiateChunk(Empty, current.Position);
            return;
        }

        /*foreach (Chunk chunk in Chunks) {
            Debug.Log(chunk.TiledMap.name);
        }*/

        int top = 1, right = 1, bottom = 1, left = 1;
        if (z + 1 < Voxels.GetLength(1)) top = Voxels[x, z + 1].value;
        if (x + 1 < Voxels.GetLength(0)) right = Voxels[x + 1, z].value;
        if (z > 0) bottom = Voxels[x, z - 1].value;
        if (x > 0) left = Voxels[x - 1, z].value;
        
        //List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in Chunks) {
            //Debug.Log(chunk.TiledMap.name);
            if (chunk.top == top &&
                chunk.right == right &&
                chunk.bottom == bottom &&
                chunk.left == left) {
                InstantiateChunk(chunk, current.Position);
                return;
            }
        }
    }

    void InstantiateChunk(Chunk chunk, Vector3 position) {
        GameObject instance = Instantiate<GameObject>(Loader);
        instance.transform.position = position;
        TiledLoader tl = instance.GetComponent<TiledLoader>();
        tl.Map = chunk.TiledMap;
        //Debug.Log(chunk.TiledMap.name);
        tl.Build();
        tl.transform.eulerAngles = Vector3.up * chunk.rotation * 90;
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
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                // activate voxels that should have roads
                if (perlin > CityMinPerlin && distance < CityRadius) Voxels[x, z].value = 2;
            }
        }
    }

    void OnDrawGizmos() {
        if (Voxels == null) return;
        foreach (Voxel v in Voxels) {
            Gizmos.color = v.value == 1 ? Color.blue : Color.black;
            Gizmos.DrawCube(v.Position, Vector3.one * 3);
        }
    }
}
