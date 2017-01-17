using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaPlace : MonoBehaviour {
	void Start () {
        transform.parent = GameManager.PizzaPlaces.transform;
	}
}
