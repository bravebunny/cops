using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;

public class GameManager : MonoBehaviour
{
	public GameObject CopPrefab;         
	public GameObject Player;         
	public float spawnInterval;
	public Camera CopCam;
	public int Layout = 0;

	private Camera PlayerCam;
	private List<GameObject> Cops = new List<GameObject> ();

	private void Start()
	{
		PlayerCam = Player.GetComponentInChildren<Camera> ();
        Physics.gravity = new Vector3(0, -30.0f, 0);
//		InvokeRepeating ("SpawnCop", 0, spawnInterval);
	}


	private void SpawnCop(Vector3 position)
	{
		position.y = 1;

		GameObject cop = Instantiate(CopPrefab, position, Player.GetComponent<Rigidbody>().rotation) as GameObject;

		cop.GetComponent<CarAIControl>().SetTarget(Player.transform);

		Cops.Add (cop);
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			print ("space key was pressed");
			SpawnCop (Player.GetComponent<Rigidbody> ().position);
		}

		if (Layout != 0 && Input.GetMouseButtonDown(0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 100;
			pos = CopCam.ScreenToWorldPoint(pos);

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
			PlayerCam.rect = new Rect(0, 0, 1, 1);
			CopCam.rect = new Rect(0, 0, 0, 0);
			break;
		case 1:
			PlayerCam.rect = new Rect(0, 0, 0, 0);
			CopCam.rect = new Rect(0, 0, 1, 1);
			break;
		case 2:
			PlayerCam.rect = new Rect(0, 0, 0.5f, 1);
			CopCam.rect = new Rect(0.5f, 0, 0.5f, 1);
			break;
		}
	}
}