using UnityEngine;
using System.Collections;

public class ColorFromList : MonoBehaviour {
    public Material[] Materials; // randomize parts of the object with this material
    public Color[] Colors; // colors to choose from
    
    void Start() {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++) {
                Material instance = renderer.materials[i];
                foreach (Material material in Materials) {
                    if (instance.name == material.name + " (Instance)") Randomize(instance);
                }

            }
        }
    }

    void Randomize(Material material) {
        int index = Random.Range(0, Colors.Length);
        material.color = Colors[index];
    }
}
