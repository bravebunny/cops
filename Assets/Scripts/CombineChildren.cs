using UnityEngine;
using System.Collections.Generic;
using System;

[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildren : MonoBehaviour {

    public void Combine() {
        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        Dictionary<string, List<CombineInstance>> combines = new Dictionary<string, List<CombineInstance>>();
        Dictionary<string, Material> namedMaterials = new Dictionary<string, Material>();
        Dictionary<string, int> vertexCount = new Dictionary<string, int>();
        Dictionary<string, int> matIndexes = new Dictionary<string, int>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        List<GameObject> toDestroy = new List<GameObject>();

        foreach (var meshRenderer in meshRenderers) {
            foreach (var material in meshRenderer.sharedMaterials)
                if (material != null && !combines.ContainsKey(material.name + "0")) {
                    string name = material.name;
                    combines.Add(name + "0", new List<CombineInstance>());
                    vertexCount.Add(name, 0);
                    matIndexes.Add(name, 0);
                    namedMaterials.Add(name, material);
                }
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach (var filter in meshFilters) {
            if (filter.gameObject.GetComponent<Road>() != null || filter.tag == "CopSpawn") continue;
            toDestroy.Add(filter.gameObject);

            if (filter.sharedMesh == null)
                continue;
            var filterRenderer = filter.GetComponent<Renderer>();
            if (filterRenderer == null || filterRenderer.sharedMaterial == null)
                continue;
            if (filterRenderer.sharedMaterials.Length > 1)
                continue;
            CombineInstance ci = new CombineInstance {
                mesh = filter.sharedMesh,
                transform = myTransform * filter.transform.localToWorldMatrix
            };

            // keep count of the vertex count for each material to avoid going over the vertex limit
            string name = filterRenderer.sharedMaterial.name;
            vertexCount[name] += filter.sharedMesh.vertexCount;
            if (vertexCount[name] >= UInt16.MaxValue) {
                matIndexes[name]++;
                vertexCount[name] = filter.sharedMesh.vertexCount;
                combines.Add(name + matIndexes[name], new List<CombineInstance>());
            };
            combines[name + matIndexes[name]].Add(ci);

            DestroyImmediate(filterRenderer);
        }

        foreach (Material m in namedMaterials.Values) {
            for (int i = 0; i <= matIndexes[m.name]; i++) {
                var go = new GameObject(m.name);
                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;

                var filter = go.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                mesh.CombineMeshes(combines[m.name + i].ToArray(), true, true);
                filter.sharedMesh = mesh;

                var arenderer = go.AddComponent<MeshRenderer>();
                arenderer.material = m;

                go.AddComponent<MeshCollider>();
            }
        }

        foreach (GameObject g in toDestroy) {
            DestroyImmediate(g);
        }
    }
}