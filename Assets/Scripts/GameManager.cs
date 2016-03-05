using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public GameObject CopPrefab;
    public GameObject SpikesPrefab;
    public GameObject LocalPlayerPrefab;
    public Canvas UI;
    public Slider BustedSlider;
    public Text EndText;
    public Text RoundText;
    public Text PlayerAWinsText;
    public Text PlayerBWinsText;
    public int MinCopDistance = 10;
    public bool AutoSpawnCops = false;

    public static bool isLocalGame = true;
    public static GameObject StaticCopPrefab;
    public static GameObject StaticSpikesPrefab;
    public static GameObject Car;
    public static Camera CarCamera;
    public static Camera CopCamera;
    public static GameObject CopPanel;

    public static int Layout = 0;
    public static int Round = 0;
    public static string Weapon = "CAR";

    static public List<NetworkPlayer> Players = new List<NetworkPlayer>();
    static public NetworkPlayer CarPlayer;
    static public NetworkPlayer CopPlayer;
    static public List<GameObject> Cops = new List<GameObject> ();
    static public GameManager sInstance = null;

    private bool duplicate = false;
    private bool roundEnded = false;


    void Awake() {
        sInstance = this;

        CarCamera = GameObject.Find("CarCamera").GetComponent<Camera>();
        CopCamera = GameObject.Find("CopCamera").GetComponent<Camera>();
        CopPanel = GameObject.Find("CopPanel");

        var managers = GameObject.FindObjectsOfType<GameManager>();
        var canvases = GameObject.FindGameObjectsWithTag("UI");

        if (managers.Length == 1) {
            // this is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(UI);
        } else {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
            Destroy(canvases[1].gameObject);
            duplicate = true;
            return;
        }

        StaticCopPrefab = CopPrefab;
        StaticSpikesPrefab = SpikesPrefab;
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);
        Time.timeScale = 1;

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

    void OnLevelWasLoaded() {
        if (duplicate)
            return;

        StartRound();
    }

    void StartRound () {
        Time.timeScale = 1;
        EndText.gameObject.SetActive(false);
        roundEnded = false;
        Round++;
        SetPlayerTypes();

        RoundText.text = "ROUND #" + Round.ToString();

        Layout = 2 + Round % 2;
    }

    public static int RegisterPlayer(NetworkPlayer player) {
        Players.Add(player);

        if (Players.Count == 2) {
            SetPlayerTypes();
        }

        return Players.IndexOf(player);
    }


    public static GameObject SpawnCop(Vector3 position) {
        if (Weapon == "CAR") {
            GameObject cop = Instantiate(StaticCopPrefab, position, Car.GetComponent<Rigidbody>().rotation) as GameObject;

            cop.GetComponent<CarAIController>().SetTarget(Car.transform);

            Cops.Add (cop);

            return cop;
        }

        GameObject spike = Instantiate(StaticSpikesPrefab, position, Quaternion.identity) as GameObject;

        return spike;
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
        else if (Input.GetKeyDown (KeyCode.Alpha4)) {
            Layout = 3;
        }

        if (CarCamera != null && CopCamera != null)
            UpdateCamera ();

        if (roundEnded)
            return;


        if (CarPlayer.insideGarage) {
            EndRound(false);
        } else if (CarPlayer.bustedLevel > 0) {
            BustedSlider.gameObject.SetActive(true);
            BustedSlider.value = CarPlayer.bustedLevel;

            if (CarPlayer.bustedLevel >= BustedSlider.maxValue) {
                EndRound(true);
            }
        } else {
            BustedSlider.gameObject.SetActive(false);
        }

        if (AutoSpawnCops) TriggerSpawner();
    }

    void EndRound (bool busted) {
        Time.timeScale = 0;
        EndText.gameObject.SetActive(true);
        roundEnded = true;

        if (busted) {
            EndText.text = "COP WINS!";
            CopPlayer.Wins++;
        } else {
            EndText.text = "CAR WINS!";
            CarPlayer.Wins++;
        }

        PlayerAWinsText.text = WinsText(Players[0].Wins);
        PlayerBWinsText.text = WinsText(Players[1].Wins);
    }

    string WinsText (int wins) {
        if (wins == 1)
            return wins.ToString() + " WIN";

        return wins.ToString() + " WINS";
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
            case 3:
                CarCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
                CopCamera.rect = new Rect(0, 0, 0.5f, 1);
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

    GameObject GetClosestSpawner() {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("CopSpawn");
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = Car.transform.position;
        foreach (GameObject potentialTarget in spawners) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && dSqrToTarget < MinCopDistance) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    void TriggerSpawner() {
        GameObject spawner = GetClosestSpawner();
        if (spawner == null) return;

        spawner.tag = "CopSpawnDisabled";
        Vector3 position = spawner.transform.position;

        StartCoroutine(SpawnMultipleCops(10, position, -spawner.transform.eulerAngles, 0.2f));
    }

    IEnumerator SpawnMultipleCops(int quantity, Vector3 position, Vector3 eulerAngles, float rate) {
        for (int i = 0; i < quantity; i++) {
            GameObject cop = SpawnCop(position);
            cop.transform.eulerAngles = eulerAngles;
            yield return new WaitForSeconds(rate);
        }
    }

    public void SelectCar () {
        Weapon = "CAR";
    }

    public void SelectSpikes () {
        Weapon = "SPIKES";
    }
}