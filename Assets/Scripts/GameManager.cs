using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Canvas UI;
    public Slider BustedSlider;
    public Text EndText;
    public Text RoundText;
    public Text PlayerAWinsText;
    public Text PlayerBWinsText;
    public Text ScoreText;

    public static bool isLocalGame = true;
    public static CarUserController Player;
    public CarUserController PlayerInstance;
    public static int Score;

    static public GameManager sInstance = null;

    bool duplicate = false;
    bool roundEnded = false;


    void Awake() {
        sInstance = this;
        Player = PlayerInstance;

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
        isLocalGame = (lobby == null);
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
        Score = 0;
        UpdateScoreText();
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (roundEnded)
            return;


        if (Player.BustedLevel > 0) {
            BustedSlider.gameObject.SetActive(true);
            BustedSlider.value = Player.BustedLevel;

            if (Player.BustedLevel >= BustedSlider.maxValue) {
                EndRound(true);
            }
        } else {
            BustedSlider.gameObject.SetActive(false);
        }
        UpdateScoreText();
    }

    void UpdateScoreText() {
        ScoreText.GetComponent<Text>().text = "Score: " + Score;
    }

    void EndRound (bool busted) {
        Time.timeScale = 0;
        EndText.gameObject.SetActive(true);
        roundEnded = true;

        /*if (busted) {
            EndText.text = "COP WINS!";
            CopPlayer.Wins++;
        } else {
            EndText.text = "CAR WINS!";
            CarPlayer.Wins++;
        }

        PlayerAWinsText.text = WinsText(Players[0].Wins);
        PlayerBWinsText.text = WinsText(Players[1].Wins);*/
    }

    string WinsText (int wins) {
        if (wins == 1)
            return wins.ToString() + " WIN";

        return wins.ToString() + " WINS";
    }
}