using UnityEngine;
using System.Collections;

public class SpawnCargo : MonoBehaviour {

    public GameObject CargoVan;
    public bool CargonOnVan;

    private MissionsAbstract Mission;

    public void Spawn(GameObject Cargo, MissionsAbstract mission) {
        GameObject cargo = Instantiate<GameObject>(Cargo);
        cargo.transform.position = CargoVan.transform.position;
        CargonOnVan = true;
        Mission = mission;
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Cargo") {
            Debug.Log("cargo off");
            CargonOnVan = false;
            Mission.MissionFailed();
        }
    }
}