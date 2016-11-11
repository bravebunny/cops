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

    public GameObject PlayerPrefab;
    public static CarUserController Player;
    public Transform StartPosition;

    public Camera CameraInstance;
    public static Camera GameCamera;

    public static int Score;
    public static int CopCount;
    public static int BombCount = 10;

    public static GameManager sInstance = null;

    bool duplicate = false;
    bool roundEnded = false;
    float InitialTimeScale;

    void Awake() {
        InitialTimeScale = Time.timeScale;
        sInstance = this;
        GameObject playerObject = Instantiate<GameObject>(PlayerPrefab);
        playerObject.transform.position = StartPosition.position;
        playerObject.transform.rotation = StartPosition.transform.rotation;
        Player = playerObject.GetComponent<CarUserController>();
        StartPosition.gameObject.SetActive(false);
        GameCamera = CameraInstance;
        CameraInstance.GetComponent<CameraController>().target = Player.transform;
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -30.0f, 0);
        Time.timeScale = InitialTimeScale;
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