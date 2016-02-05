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

	private List<GameObject> Cops = new List<GameObject> ();

	private void Start()
	{
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

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = 100;
			pos = CopCam.ScreenToWorldPoint(pos);

			SpawnCop (pos);

		}
	}
}