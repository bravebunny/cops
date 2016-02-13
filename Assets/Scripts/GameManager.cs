using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {
    public GameObject CopPrefab;
    public GameObject Car;
    public Camera CarCamera;
    public Camera CopCamera;
    public int Layout = 0;

    private List<GameObject> Cops = new List<GameObject> ();

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);
    }


    private void SpawnCop(Vector3 position) {
        position.y = 2;

        GameObject cop = Instantiate(CopPrefab, position, Car.GetComponent<Rigidbody>().rotation) as GameObject;

        cop.GetComponent<CarAIController>().SetTarget(Car.transform);

        NetworkServer.Spawn(cop);
        Cops.Add (cop);
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Layout != 0 && Input.GetMouseButtonDown(0)) {
            Vector3 pos = Input.mousePosition;
            pos.z = 100;
            pos = CopCamera.ScreenToWorldPoint(pos);

            SpawnCop (pos);
        }

        UpdateCamera ();
    }

    private void UpdateCamera () {
        if (Input.GetKeyDown (KeyCode.Alpha1)) {
            Layout = 0;
        }
        else if (Input.GetKeyDown (KeyCode.Alpha2)) {
            Layout = 1;
        }
        else if (Input.GetKeyDown (KeyCode.Alpha3)) {
            Layout = 2;
        }

        switch (Layout) {
            case 0:
                CarCamera.rect = new Rect(0, 0, 1, 1);
                CopCamera.rect = new Rect(0, 0, 0, 0);
                break;
            case 1:
                CarCamera.rect = new Rect(0, 0, 0, 0);
                CopCamera.rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                CarCamera.rect = new Rect(0, 0, 0.5f, 1);
                CopCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
        }
    }
}