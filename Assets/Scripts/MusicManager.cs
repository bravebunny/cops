using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
    public AudioSource Jump; // music that plays while player is airborn
    public AudioSource Main; // always plays
    public AudioSource Movement; // plays when moving
    public float MinMovementSpeed = 5; // minimum speed to trigger movement music
    public float FadeSpeed = 0.1f;
    public float MaxVolume = 0.7f;
    CarController Player;

    void Start() {
        Player = GameManager.Player.GetComponent<CarController>();
        Main.volume = MaxVolume;
        Jump.volume = 0;
        Movement.volume = 0;
    }

	void Update () {
        if (Player.Grounded) FadeOut(Jump);
        else FadeIn(Jump);

        if (Mathf.Abs(Player.CurrentSpeed) > MinMovementSpeed) FadeIn(Movement);
        else FadeOut(Movement);
    }

    void FadeIn(AudioSource music) {
        if (music.volume < MaxVolume) music.volume += FadeSpeed;
        else if (music.volume > MaxVolume) music.volume = MaxVolume;
    }

    void FadeOut(AudioSource music) {
        if (music.volume > 0) music.volume -= FadeSpeed;
    }
}
