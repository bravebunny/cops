using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
    public AudioSource Jump; // music that plays while player is airborn
    public AudioSource Main; // always plays
    public AudioSource Movement; // plays when moving
    public AudioSource FewCops; // plays when there are a few cops around
    public AudioSource ManyCops; // plays when there are many cops around

    public int MinCopsFew = 1;
    public int MinCopsMany = 10;
    public float MinMovementSpeed = 5; // minimum speed to trigger movement music
    public float FadeSpeed = 0.1f;
    public float QuietVolume = 0.3f;
    public float LoudVolume = 1f;

    CarController Player;
    CarUserController PlayerUser;

    void Start() {
        Player = GameManager.Player.GetComponent<CarController>();
        PlayerUser = GameManager.Player.GetComponent<CarUserController>();
        Main.volume = QuietVolume;
        Jump.volume = 0;
        Movement.volume = 0;
        ManyCops.volume = 0;
        FewCops.volume = 0;
    }

	void Update () {
        if (Player.Grounded) FadeOut(Jump);
        else FadeIn(Jump, QuietVolume);

        if (Mathf.Abs(Player.CurrentSpeed) > MinMovementSpeed) FadeIn(Movement, QuietVolume);
        else FadeOut(Movement);

        if (PlayerUser.CopCount >= MinCopsFew) FadeIn(FewCops, LoudVolume);
        else FadeOut(FewCops);

        if (PlayerUser.CopCount >= MinCopsMany) FadeIn(ManyCops, LoudVolume);
        else FadeOut(ManyCops);
    }

    void FadeIn(AudioSource music, float targetVolume) {
        if (music.volume < targetVolume) music.volume += FadeSpeed;
        else if (music.volume > targetVolume) music.volume = QuietVolume;
    }

    void FadeOut(AudioSource music) {
        if (music.volume > 0) music.volume -= FadeSpeed;
    }
}
