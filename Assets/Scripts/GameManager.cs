using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public GameObject CopPrefab;
    public GameObject LocalPlayerPrefab;
    public Slider BustedSlider;
    public Text EndText;

    public static bool isLocalGame = true;
    public static GameObject StaticCopPrefab;
    public static GameObject Car;
    public static Camera CarCamera;
    public static Camera CopCamera;
    public static int Layout = 0;
    public static int Round = 0;

    static public List<NetworkPlayer> Players = new List<NetworkPlayer>();
    static public NetworkPlayer CarPlayer;
    static public NetworkPlayer CopPlayer;
    static public List<GameObject> Cops = new List<GameObject> ();
    static public GameManager sInstance = null;

    void Awake() {
        sInstance = this;

        CarCamera = GameObject.Find("CarCamera").GetComponent<Camera>();
        CopCamera = GameObject.Find("CopCamera").GetComponent<Camera>();

        StaticCopPrefab = CopPrefab;
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);

        GameObject lobby = GameObject.Find("LobbyManager");
        isLocalGame = (lobby == null);

        Debug.Log("isLocalGame " + isLocalGame.ToString());

        Transform startPosition = GameObject.FindObjectOfType<NetworkStartPosition>().transform;

        if (isLocalGame) {
            Layout = 2;

            for (int i = 0; i < 2; i++) {
                Instantiate(LocalPlayerPrefab, startPosition.position, Quaternion.identity);
            }

        }
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

        GameObject cop = Instantiate(StaticCopPrefab, position, Car.GetComponent<Rigidbody>().rotation) as GameObject;

        cop.GetComponent<CarAIController>().SetTarget(Car.transform);

        Cops.Add (cop);

        return cop;
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown (KeyCode.Alpha1)) {
            Layout = 0;
        }
        else if (Input.GetKeyDown (KeyCode.Alpha2)) {
            Layout = 1;
        }
        else if (Input.GetKeyDown (KeyCode.Alpha3)) {
            Layout = 2;
        }

        if (CarCamera != null && CopCamera != null)
            UpdateCamera ();

        if (CarPlayer.bustedLevel > 0) {
            BustedSlider.gameObject.SetActive(true);
            BustedSlider.value = CarPlayer.bustedLevel;

            if (CarPlayer.bustedLevel >= BustedSlider.maxValue) {
                EndRound(true);
            }
        } else {
            BustedSlider.gameObject.SetActive(false);
        }
    }

    void EndRound (bool busted) {
        Time.timeScale = 0;
        EndText.gameObject.SetActive(true);

        if (busted) {
            EndText.text = "COP WINS!";
        } else {
            EndText.text = "CAR WINS!";
        }

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
        for(int i = 0; i < Players.Count; ++i) {
            string type = GetPlayerType(i);
            Players[i].SetType(type);

            if (type == "CAR") {
                Car = Players[i].gameObject;
                CarPlayer = Players[i];
            } else {
                CopPlayer = Players[i];
            }
        }

        CarCamera.GetComponent<CameraController>().SetTarget(Car.transform);
    }
}