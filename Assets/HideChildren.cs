using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideChildren : MonoBehaviour {

    public Shader ShaderToHide;
    List<Material> Material;
    List<MeshRenderer> MeshRend;
    bool FadeIn = false;
    bool FadeOut = false;
    Material LastMaterial;

    void Start() {

        MeshRend = new List<MeshRenderer>();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<MeshRenderer>())
                MeshRend.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
        }

        Material = new List<Material>();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Renderer>())
                Material.Add(transform.GetChild(i).GetComponent<Renderer>().material);
        }

        if(Material.Count > 0)
            LastMaterial = Material[Material.Count - 1];
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "MainCamera") {
            FadeIn = false;
            FadeOut = true;
            foreach (Material mat in Material) {
                if (mat) {
                    mat.SetFloat("_Mode", 3);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.DisableKeyword("_ALPHABLEND_ON");
                    mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
            }
            foreach (MeshRenderer mr in MeshRend) {
                if (mr) {
                    mr.shadowCastingMode = ShadowCastingMode.Off;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "MainCamera") {
            FadeIn = true;
            FadeOut = false;
            foreach (MeshRenderer mr in MeshRend) {
                if (mr) {
                    mr.shadowCastingMode = ShadowCastingMode.On;
                }
            }
        }
    }

    void FixedUpdate() {
        if (FadeIn) {
            foreach (Material mat in Material) {
                if (mat && mat.shader == ShaderToHide) {
                    Color32 saveColor = mat.GetColor("_Color");
                    if (mat == LastMaterial && saveColor.a == 255) {
                        SetOpaque();
                        FadeIn = false;
                    } else {
                        saveColor.a += 5;
                        mat.SetColor("_Color", saveColor);
                    }
                }
            }
        } else if (FadeOut) {
            foreach (Material mat in Material) {
                if (mat && mat.shader == ShaderToHide) {
                    Color saveColor = mat.color;
                    if (saveColor.a != 0) {
                        saveColor.a -= 0.07f;
                        mat.color = saveColor;
                    }
                }
            }
        }
    }

    void SetOpaque() {
        foreach (Material mat in Material) {
            if (mat) {
                Color32 saveColor = mat.GetColor("_Color");
                mat.SetFloat("_Mode", 1);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;
            }
        }
    }
}
