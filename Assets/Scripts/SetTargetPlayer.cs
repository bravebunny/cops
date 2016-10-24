using UnityEngine;
using System.Collections;

public class SetTargetPlayer : MonoBehaviour {
	void Start () {
        GetComponent<CarAIController>().Target = GameManager.Player.transform;
	}
}
