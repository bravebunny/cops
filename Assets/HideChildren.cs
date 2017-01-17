using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideChildren : MonoBehaviour {

    Material[] Material;
    bool FadeIn = false;
    bool FadeOut = false;

    void Start() {
        Material = new Material[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Renderer>())
                Material[i] = transform.GetChild(i).GetComponent<Renderer>().material;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "MainCamera") {
            FadeIn = false;
            FadeOut = true;
            foreach (Material mat in Material) {
                if (mat) {
                    //child.GetComponent<Renderer>().enabled = false;
                    mat.SetFloat("_Mode", 4);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.DisableKeyword("_ALPHABLEND_ON");
                    mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "MainCamera") {
            FadeIn = true;
            FadeOut = false;
            /*foreach (Material mat in Material) {
                if (mat) {
                    
                    Color saveColor = mat.color;
                    saveColor.a = 1.0f;
                    mat.color = saveColor;
                }
            }*/
        }
    }

    void FixedUpdate() {
        if (FadeIn) {
            foreach (Material mat in Material) {
                if (mat) {
                    Color32 saveColor = mat.GetColor("_Color");
                    if (saveColor.a != 255) {
                        saveColor.a += 5;
                        mat.SetColor("_Color", saveColor);
                    } else {
                        mat.SetFloat("_Mode", 0);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = -1;
                        FadeIn = false;
                    }
                }
            }
        } else if (FadeOut) {
            foreach (Material mat in Material) {
                if (mat) {
                    Color saveColor = mat.color;
                    if (saveColor.a != 0) {
                        saveColor.a -= 0.1f;
                        mat.color = saveColor;
                    }
                }
            }
        }
    }
}
