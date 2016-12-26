using UnityEngine;
using System.Collections;

public class SpawnCargo : MonoBehaviour {

    public GameObject CargoVan;
    public bool CargonOnVan;

    public void Spawn(GameObject Cargo) {
        GameObject cargo = Instantiate<GameObject>(Cargo);
        cargo.transform.position = CargoVan.transform.position;
        CargonOnVan = true;
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Cargo") {
            Debug.Log("cargo off");
            CargonOnVan = false;
        }
    }
}