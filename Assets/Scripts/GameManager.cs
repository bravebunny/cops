using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public static GameObject CopPrefab;
    public static GameObject Car;
    public static Camera CarCamera;
    public static Camera CopCamera;
    public static int Layout = 0;
    public static int Round = 0;

    static public List<NetworkPlayer> Players = new List<NetworkPlayer>();
    static public List<GameObject> Cops = new List<GameObject> ();
    static public GameManager sInstance = null;

    protected bool _running = true;

    void Awake()
    {
        sInstance = this;

        CarCamera = GameObject.Find("CarCamera").GetComponent<Camera>();
        CopCamera = GameObject.Find("CopCamera").GetComponent<Camera>();
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);
    }

    public static int RegisterPlayer(NetworkPlayer player) {
        Players.Add(player);

        if (Players.Count == 2) {
            SetPlayerTypes();
        }

        return Players.IndexOf(player);
    }


    public static GameObject SpawnCop(Vector3 position) {
        position.y = 2;

        GameObject cop = Instantiate(CopPrefab, position, Car.GetComponent<Rigidbody>().rotation) as GameObject;

        cop.GetComponent<CarAIController>().SetTarget(Car.transform);

        Cops.Add (cop);

        return cop;
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

//        if (Layout != 0 && Input.GetMouseButtonDown(0)) {
//            Vector3 pos = Input.mousePosition;
//            pos.z = 100;
//            pos = CopCamera.ScreenToWorldPoint(pos);
//
//            SpawnCop (pos);
//        }

//        if (CarCamera != null && CopCamera != null)
//            UpdateCamera ();
    }

    public static void SetLayoutByPlayerIndex (int index) {
        string type = GetPlayerType(index);
        if (type == "CAR")
            Layout = 0;
        else
            Layout = 1;

        UpdateCamera();
    }

    private static void UpdateCamera () {
//        if (Input.GetKeyDown (KeyCode.Alpha1)) {
//            Layout = 0;
//        }
//        else if (Input.GetKeyDown (KeyCode.Alpha2)) {
//            Layout = 1;
//        }
//        else if (Input.GetKeyDown (KeyCode.Alpha3)) {
//            Layout = 2;
//        }
//
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

    public static string GetPlayerType (int index) {
        if (Round % 2 == 1) {
            if (index % 2 == 0)
                return "COP";
            else
                return "CAR";
        } else {
            if (index % 2 == 0)
                return "CAR";
            else
                return "COP";
        }
    }

    private static void SetPlayerTypes () {
        Debug.Log("SetPlayerTypes " + Players.Count.ToString());

        for(int i = 0; i < Players.Count; ++i) {
            string type = GetPlayerType(i);
            Players[i].SetType(type);

            if (type == "CAR") {
                Debug.Log("Get Car");
                Car = Players[i].gameObject;
            }
        }
        Debug.Log(Car);

        CarCamera.GetComponent<CameraController>().SetTarget(Car.transform);
    }
}