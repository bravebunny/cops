using UnityEngine;
using System.Collections;

public class SpawnCargo : MonoBehaviour {

    public GameObject CargoVan;
    public int DestroyCargoTime = 5;

    private GameObject cargo;

    private MissionsAbstract Mission;

    public void Spawn(GameObject Cargo, MissionsAbstract mission) {
        cargo = Instantiate<GameObject>(Cargo);
        cargo.transform.position = CargoVan.transform.position;
        Mission = mission;
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Cargo") {
            Mission.MissionFailed();
            Destroy(collider, DestroyCargoTime);
        }
    }

    public void DestroyCargo() {
        Destroy(cargo);
    }
}