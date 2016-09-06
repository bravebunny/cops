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
    }
}