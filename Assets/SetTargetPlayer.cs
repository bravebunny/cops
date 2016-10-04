using UnityEngine;
using System.Collections;

public class SetTargetPlayer : MonoBehaviour {
	void Start () {
        GetComponent<AIPath>().target = GameManager.Player.transform;
	}
}
