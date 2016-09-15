using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Canvas UI;
    public Slider BustedSlider;
    public Text GameOverText;
    public Text ScoreText;
    public Text CopCountText;
    public Text BombCountText;

    public static CarUserController Player;
    public CarUserController PlayerInstance;

    public Camera CameraInstance;
    public static Camera GameCamera;

    public static int Score;
    public static int CopCount;
    public static int BombCount = 10;

    public static GameManager sInstance = null;

    bool duplicate = false;
    bool roundEnded = false;


    void Awake() {
        sInstance = this;
        Player = PlayerInstance;
        GameCamera = CameraInstance;

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
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);
        Time.timeScale = 1;
        GameObject lobby = GameObject.Find("LobbyManager");
    }

    void OnLevelWasLoaded() {
        if (duplicate)
            return;

        StartRound();
    }

    void StartRound () {
        Time.timeScale = 1;
        GameOverText.gameObject.SetActive(false);
        roundEnded = false;
        Score = 0;
        BombCount = 0;
        CopCount = 0;
        UpdateText();
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (roundEnded)
            return;


        if (Player.BustedLevel > 0) {
            BustedSlider.gameObject.SetActive(true);
            BustedSlider.value = Player.BustedLevel;

            if (Player.BustedLevel >= BustedSlider.maxValue) {
                EndRound();
            }
        } else {
            BustedSlider.gameObject.SetActive(false);
        }
        UpdateText();
    }

    void UpdateText() {
        ScoreText.text = "Score: " + Score;
        CopCountText.text = "Cops: " + CopCount;
        BombCountText.text = "Bombs: " + BombCount;
    }

    void EndRound () {
        Time.timeScale = 0;
        GameOverText.gameObject.SetActive(true);
        roundEnded = true;
        GetComponent<AudioSource>().Play();
    }
}