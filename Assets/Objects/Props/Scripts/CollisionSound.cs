using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio.isPlaying) return;

        audio.pitch = Random.Range(-2f, 2f);
        audio.volume = collision.relativeVelocity.magnitude/10;
        audio.Play();
    }
}
