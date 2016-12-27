using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RandomMap))]
public class RandomMapEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        RandomMap tiled = (RandomMap)target;
        if (GUILayout.Button("Generate")) {
            tiled.Generate();
        }
        if (GUILayout.Button("Clear")) {
            tiled.Clear();
        }
    }
}