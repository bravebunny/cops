using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizeColor : MonoBehaviour {
    public List<Material> Materials; // randomize parts of the object with this material

	// Use this for initialization
	void Start () {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++) {
                Material shared = renderer.sharedMaterials[i];
                Material instance = renderer.materials[i];
                if (Materials.Contains(shared)) Randomize(instance);
            }
        }
	}

    void Randomize(Material material) {
        float r = Random.value;
        float g = Random.value;
        float b = Random.value;
        material.color = new Color(r, g, b);
    }
}
