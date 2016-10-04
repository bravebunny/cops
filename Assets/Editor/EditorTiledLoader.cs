using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TiledLoader))]
public class ObjectBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        TiledLoader myScript = (TiledLoader)target;
        if (GUILayout.Button("Load Map")) {
            myScript.Build();
            NavMeshBuilder.BuildNavMeshAsync();
        }
        if (GUILayout.Button("Clear")) {
            myScript.Clear();
            NavMeshBuilder.ClearAllNavMeshes();
        }
        if (GUILayout.Button("Update A*")) {
            NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
            Mesh mesh = new Mesh();
            mesh.vertices = triangles.vertices;
            mesh.triangles = triangles.indices;
            
            AssetDatabase.CreateAsset(mesh, "Assets/Resources/navmesh.asset");
            AssetDatabase.SaveAssets();
        }
    }
}