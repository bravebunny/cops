using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RandomMap : MonoBehaviour {
    public TextAsset[] BaseChunks;
    public TextAsset[] BaseGroundChunks;
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

    public Chunk[] CityChunks;
    public Chunk[] GroundChunks;

    const int EMPTY = 0, GROUND = 1, ROAD = 2;

    Voxel[,] CityVoxels;
    Voxel[,] IslandVoxels;
    float Seed;
    int SeedRange = 10000;
    float SeedOffset = 0.5f;


    public class Voxel {
        public int Value = EMPTY;
        public Vector3 Position;
        public Vector3 ZCorner;
        public Vector3 XCorner;

        public Voxel(int x, int z, float size, int value) {
            Value = value;
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

        CityChunks = new Chunk[BaseChunks.Length * 4];
        GroundChunks = new Chunk[BaseGroundChunks.Length * 4];

        for (int i = 0; i < BaseChunks.Length; i++) {
            for (int rotation = 0; rotation < 4; rotation++) {
                Chunk newChunk = new Chunk(BaseChunks[i]);
                newChunk.SetRotation(rotation);
                CityChunks[i * 4 + rotation] = newChunk;
            }
        }

        for (int i = 0; i < BaseGroundChunks.Length; i++) {
            for (int rotation = 0; rotation < 4; rotation++) {
                Chunk newChunk = new Chunk(BaseGroundChunks[i]);
                newChunk.SetRotation(rotation);
                GroundChunks[i * 4 + rotation] = newChunk;
            }
        }

        CityVoxels = new Voxel[SizeX, SizeZ];
        IslandVoxels = new Voxel[SizeX, SizeZ];
        float size = ChunkSize * TileSize;
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                CityVoxels[x, z] = new Voxel(x, z, size, GROUND);
                IslandVoxels[x, z] = new Voxel(x, z, size, EMPTY);
            }
        }

        PopulateVoxels(CityMinPerlin, CityRadius, CityVoxels, ROAD, 1);
        PopulateVoxels(IslandMinPerlin, IslandRadius, IslandVoxels, GROUND, 4);


        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                distance *= distance * distance * distance;
                // create chunks only inside the island
                //if (perlin > IslandMinPerlin + IslandRadius * distance) CreateChunk(x, z);

                CreateIslandChunk(x, z);
            }
        }
    }

    void CreateIslandChunk(int x, int z) {

        Voxel current = IslandVoxels[x, z];
        /*if (current.Value == EMPTY) {
            return;
        }*/

        /*foreach (Chunk chunk in Chunks) {
            Debug.Log(chunk.TiledMap.name);
        }*/

        int top, right, bottom, left;
        top = right = bottom = left = EMPTY;
        if (z + 1 < IslandVoxels.GetLength(1) && x + 1 < IslandVoxels.GetLength(0)) top = IslandVoxels[x + 1, z + 1].Value;
        if (x + 1 < IslandVoxels.GetLength(0)) right = IslandVoxels[x + 1, z].Value;
        bottom = IslandVoxels[x, z].Value;
        if (z + 1 < IslandVoxels.GetLength(1)) left = IslandVoxels[x, z + 1].Value;

        //List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in GroundChunks) {
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

    void CreateChunk(int x, int z) {

        Voxel current = CityVoxels[x, z];
        if (current.Value == GROUND) {
            return;
        }

        /*foreach (Chunk chunk in Chunks) {
            Debug.Log(chunk.TiledMap.name);
        }*/

        int top, right, bottom, left;
        top = right = bottom = left = GROUND;
        if (z + 1 < CityVoxels.GetLength(1)) top = CityVoxels[x, z + 1].Value;
        if (x + 1 < CityVoxels.GetLength(0)) right = CityVoxels[x + 1, z].Value;
        if (z > 0) bottom = CityVoxels[x, z - 1].Value;
        if (x > 0) left = CityVoxels[x - 1, z].Value;
        
        //List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in CityChunks) {
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

    void PopulateVoxels(float minPerlin, float radius, Voxel[,] voxels, int value, float distanceFactor) {
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                distance = Mathf.Pow(distance, distanceFactor);
                perlin /= distance;
                // activate voxels that should have roads
                if (perlin > minPerlin /*&& distance < radius*/) voxels[x, z].Value = value;
            }
        }
    }

    void OnDrawGizmos() {
        if (IslandVoxels == null) return;
        foreach (Voxel v in IslandVoxels) {
            Color color;
            if (v.Value == EMPTY) color = Color.blue;
            else if (v.Value == GROUND) color = Color.green;
            else color = Color.black;
            Gizmos.color = color;
            Gizmos.DrawCube(v.Position, Vector3.one * 3);
        }
    }
}
