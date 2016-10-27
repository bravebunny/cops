using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TiledLoader))]
public class ObjectBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        TiledLoader tiled = (TiledLoader)target;
        if (GUILayout.Button("Load Map")) {
            tiled.Build();
        }
        if (GUILayout.Button("Clear")) {
            tiled.Clear();
        }
    }
}