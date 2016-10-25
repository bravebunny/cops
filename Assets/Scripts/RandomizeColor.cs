using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizeColor : MonoBehaviour {
    public List<Material> Materials; // randomize parts of the object with this material
    public int MinHue = 0;
    public int MaxHue = 360;
    public int MinSat = 0;
    public int MaxSat = 255;
    public int MinVal = 0;
    public int MaxVal = 255;

    // Use this for initialization
    void Start () {
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
        float h = Random.Range((float)MinHue, (float)MaxHue) / 360;
        float s = Random.Range((float)MinSat, (float)MaxSat) / 255;
        float v = Random.Range((float)MinVal, (float)MaxVal) / 255;
        material.color = Color.HSVToRGB(h, s, v);
    }
}
