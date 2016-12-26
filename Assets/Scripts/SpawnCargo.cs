using UnityEngine;
using System.Collections;

public class SpawnCargo : MonoBehaviour {

    public Transform CargoVan;
	
    public void Spawn(GameObject Cargo) {
       GameObject cargo = Instantiate<GameObject>(Cargo);
       cargo.transform.position = CargoVan.position;
    }
}
