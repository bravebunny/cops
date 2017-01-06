using UnityEngine;
using System.Collections;

public class SpawnCargo : MonoBehaviour {

    public GameObject CargoVan;
    public int DestroyCargoTime = 5;

    private GameObject cargo;

    private MissionManager MissionManager;

    public void Spawn(GameObject Cargo, MissionManager missionManager) {
        cargo = Instantiate<GameObject>(Cargo);
        cargo.transform.position = CargoVan.transform.position;
        MissionManager = missionManager;
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Cargo") {
            MissionManager.MissionFailed();
            Destroy(collider, DestroyCargoTime);
        }
    }

    public void DestroyCargo() {
        Destroy(cargo);
    }
}