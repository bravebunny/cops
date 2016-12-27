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
    Vector3 Center;

    public float CityRadius = 5;
    public float CityMinPerlin = 0.35f;
    public float IslandRadius = 0.0001f;
    public float IslandMinPerlin = 0.05f;
    public int SizeY = 3;
    public float LevelHeight = 4;
    public Material[] LevelMaterials;

    public Chunk[] CityChunks;
    public Chunk[] GroundChunks;

    const int EMPTY = 0, GROUND = 1, ROAD = 2;

    Voxel[,] CityVoxels;
    Voxel[][,] IslandVoxels;

    Transform[] Island;

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
        }

        public void SetRotation(int rotation) {
            for (int i = 0; i < rotation; i++) Rotate();
        }

        // rotating a voxel means shifting all the values to the right
        public void Rotate() {
            int oldTop = top;
            top = left;
            left = bottom;
            bottom = right;
            right = oldTop;
            rotation++;
        }

        public override string ToString() {
            return TiledMap.name + ": " + top + "," + right + "," + bottom + "," + left;
        }
    }

    public void Generate () {
        Clear();

        float width = SizeX * TileSize * ChunkSize;
        float height = SizeZ * TileSize * ChunkSize;
        Center = new Vector3(width / 2, 0, height / 2);

        // create a large integer number for the perlin noise seed
        // the offset isn't random because it affects the density of the noise
        Seed = Random.Range(1, SeedRange) + SeedOffset;

        CityChunks = new Chunk[BaseChunks.Length * 4];
        GroundChunks = new Chunk[BaseGroundChunks.Length * 4];

        // initalize city chunks and their rotated versions
        for (int i = 0; i < BaseChunks.Length; i++) {
            for (int rotation = 0; rotation < 4; rotation++) {
                Chunk newChunk = new Chunk(BaseChunks[i]);
                newChunk.SetRotation(rotation);
                CityChunks[i * 4 + rotation] = newChunk;
            }
        }

        // initalize island chunks and their rotated versions
        for (int i = 0; i < BaseGroundChunks.Length; i++) {
            for (int rotation = 0; rotation < 4; rotation++) {
                Chunk newChunk = new Chunk(BaseGroundChunks[i]);
                newChunk.SetRotation(rotation);
                GroundChunks[i * 4 + rotation] = newChunk;
            }
        }

        // initialize voxels
        CityVoxels = new Voxel[SizeX, SizeZ];
        IslandVoxels = new Voxel[SizeY][,];
        for (int i = 0; i < SizeY; i++) {
            IslandVoxels[i] = new Voxel[SizeX, SizeZ];
        }

        float size = ChunkSize * TileSize;
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                CityVoxels[x, z] = new Voxel(x, z, size, GROUND);
                for (int y = 0; y < SizeY; y++)
                    IslandVoxels[y][x, z] = new Voxel(x, z, size, EMPTY);
            }
        }

        // assign pseudo-random values to the voxels
        PopulateVoxels(CityMinPerlin, CityRadius, CityVoxels, ROAD, 1);

        for (int y = 0; y < SizeY; y++)
            PopulateVoxels(IslandMinPerlin / ((y + 1)), IslandRadius, IslandVoxels[y], GROUND, 5);

        // avoid having roads on top of island holes on the topmost level, 0
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                if (CityVoxels[x, z].Value != GROUND) {
                    SetIslandVoxel(x, 0, z, GROUND);
                    SetIslandVoxel(x + 1, 0, z, GROUND);
                    SetIslandVoxel(x, 0, z + 1, GROUND);
                    SetIslandVoxel(x + 1, 0, z + 1, GROUND);
                }
                for (int y = 1; y < SizeY; y++) {
                    if (IslandVoxels[y-1][x, z].Value == GROUND) {
                        SetIslandVoxel(x, y, z, GROUND);
                        SetIslandVoxel(x + 1, y, z, GROUND);
                        SetIslandVoxel(x, y, z + 1, GROUND);
                        SetIslandVoxel(x + 1, y, z + 1, GROUND);
                        SetIslandVoxel(x - 1, y, z, GROUND);
                        SetIslandVoxel(x, y, z - 1, GROUND);
                        SetIslandVoxel(x - 1, y, z - 1, GROUND);
                    }
                }
            }
        }

        // create the parent objects
        Island = new Transform[SizeY];
        for (int y = 0; y < SizeY; y++) {
            Island[y] = new GameObject().transform;
            Island[y].name = "Layer" + y;
            Island[y].parent = transform;
        }


        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                distance *= distance * distance * distance;
                CreateCityChunk(x, z);
                for (int y = 0; y < SizeY; y++) {
                    CreateIslandChunk(x, y, z);
                }
            }
        }

        // combine each layer
        for (int y = 0; y < SizeY; y++) {
            //Island[y].gameObject.AddComponent<CombineChildren>().Combine();
            if (LevelMaterials[y]) {
                MeshRenderer[] renderers = Island[y].GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer r in renderers) {
                    r.material = LevelMaterials[y];
                }
            }
        }
    }

    void SetIslandVoxel(int x, int y, int z, int value) {
        // check if inside bounds
        if (x < 0 || x >= SizeX ||
            y < 0 || y >= SizeY ||
            z < 0 || z >= SizeZ)
            return;
       IslandVoxels[y][x, z].Value = value;
    }

    void CreateIslandChunk(int x, int y, int z) {
        Voxel current = IslandVoxels[y][x, z];
        if (CityVoxels[x, z].Value != GROUND) return;

        // look around this voxel to decide which chunks are valid
        int top, right, bottom, left;
        top = right = bottom = left = EMPTY;
        if (z + 1 < IslandVoxels[y].GetLength(1) && x + 1 < IslandVoxels[y].GetLength(0)) top = IslandVoxels[y][x + 1, z + 1].Value;
        if (x + 1 < IslandVoxels[y].GetLength(0)) right = IslandVoxels[y][x + 1, z].Value;
        bottom = IslandVoxels[y][x, z].Value;
        if (z + 1 < IslandVoxels[y].GetLength(1)) left = IslandVoxels[y][x, z + 1].Value;

        // pick a valid chunk for this voxel
        //List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in GroundChunks) {
            if (chunk.top == top &&
                chunk.right == right &&
                chunk.bottom == bottom &&
                chunk.left == left) {
                InstantiateChunk(chunk, current.Position - Vector3.up * y * LevelHeight, Island[y]);
                return;
            }
        }
    }

    void CreateCityChunk(int x, int z) {
        Voxel current = CityVoxels[x, z];
        if (current.Value == GROUND) return;

        // look around this voxel to decide which chunks are valid
        int top, right, bottom, left;
        top = right = bottom = left = GROUND;
        if (z + 1 < CityVoxels.GetLength(1)) top = CityVoxels[x, z + 1].Value;
        if (x + 1 < CityVoxels.GetLength(0)) right = CityVoxels[x + 1, z].Value;
        if (z > 0) bottom = CityVoxels[x, z - 1].Value;
        if (x > 0) left = CityVoxels[x - 1, z].Value;
        
        // pick a valid chunk for this voxel
        //List<Chunk> validChunks = new List<Chunk>();
        foreach (Chunk chunk in CityChunks) {
            if (chunk.top == top &&
                chunk.right == right &&
                chunk.bottom == bottom &&
                chunk.left == left) {
                InstantiateChunk(chunk, current.Position, Island[0]);
                return;
            }
        }
    }

    void InstantiateChunk(Chunk chunk, Vector3 position, Transform parent) {
        GameObject instance = Instantiate<GameObject>(Loader);
        instance.transform.position = position - Center;
        TiledLoader tl = instance.GetComponent<TiledLoader>();
        tl.Map = chunk.TiledMap;
        //Debug.Log(chunk.TiledMap.name);
        tl.Build();
        tl.transform.eulerAngles = Vector3.up * chunk.rotation * 90;
        instance.transform.parent = parent;
    }

    void PopulateVoxels(float minPerlin, float radius, Voxel[,] voxels, int value, float distanceFactor) {
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                float perlin = Mathf.PerlinNoise(x + Seed, z + Seed);
                float distance = Vector2.Distance(new Vector2(x, z), new Vector2(SizeX / 2, SizeZ / 2));
                if (distanceFactor == 1) { // for roads
                    if (perlin > minPerlin && distance < radius) voxels[x, z].Value = value;
                } else { // for land
                    distance = Mathf.Pow(distance, distanceFactor);
                    perlin /= distance;
                    if (perlin > minPerlin) voxels[x, z].Value = value;
                }
            }
        }
    }

    void OnDrawGizmos() {
        /*for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                Vector3 position = IslandVoxels[x, z].Position;
                if (CityVoxels[x, z].Value == ROAD) {
                    position = CityVoxels[x, z].Position;
                    Gizmos.color = Color.black;
                } else if (IslandVoxels[x, z].Value == GROUND) Gizmos.color = Color.green;
                else Gizmos.color = Color.blue;
                Gizmos.DrawCube(position, Vector3.one * TileSize * ChunkSize / 5);
            }
        }*/

    }

    public void Clear() {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }
}
