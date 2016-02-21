using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayer : NetworkBehaviour {
    public string Type = null;
    public int Index;

    public static bool isLocalGame;

    protected Rigidbody _rigidbody;
    protected CarController _carControlller; // the car controller we want to use

    public int bustedLevel = 0;

    float steering;
    float accel;

    void Awake() {
        //register the player in the gamemanager, that will allow to loop on it.
        Index = GameManager.RegisterPlayer(this);

        isLocalGame = GameManager.isLocalGame;

        EnableCar(false);

        _rigidbody = GetComponent<Rigidbody>();
        _carControlller = GetComponent<CarController>();
    }
        
    // Use this for initialization
    void Start () {
        if (Type != null) {
            EnableCar(Type == "CAR");
        }

        if (isLocalPlayer && !isLocalGame)
            GameManager.SetLayoutByPlayerIndex(Index);
    }

    void OnCollisionStay(Collision collisionInfo) {
        if (collisionInfo.collider.tag != "Cop")
            return;

        bustedLevel += 5;
    }


//    [ClientCallback]
    void Update() {
        if (!isLocalPlayer && !isLocalGame) {
            return;
        }

        if (Type == "CAR") {
            steering = CrossPlatformInputManager.GetAxis("Horizontal");
            accel = CrossPlatformInputManager.GetAxis("Vertical");

            if (bustedLevel > 0) {
                bustedLevel--;
//                print("bustedLevel " + bustedLevel.ToString());
            }
        } else {
            if (GameManager.Layout != 0 && Input.GetMouseButtonDown(0)) {
                Vector3 pos = Input.mousePosition;
                pos.z = 100;
                pos = GameManager.CopCamera.ScreenToWorldPoint(pos);

                // we call a Command, that will be executed only on server, to spawn a new cop
                if (!isLocalGame)
                    CmdSpawnCop(pos);
                else
                    SpawnCop(pos);
            }
        }
    }


//    [ClientCallback]
    void FixedUpdate() {
        if (!isLocalPlayer && !isLocalGame)
            return;

        if (Type == "CAR") {
            _carControlller.Move(steering, accel);
        }
    }

    public void SpawnCop(Vector3 position) {
        GameObject cop = GameManager.SpawnCop(position);

        if (isLocalGame)
            return;

        NetworkServer.Spawn(cop);
        // NetworkServer.SpawnWithClientAuthority(cop, connectionToClient);
    }

    [Command]
    public void CmdSpawnCop(Vector3 position) {
        if (isServer) { // avoid to create bullet twice (here & in Rpc call) on hosting client
            SpawnCop(position);
        }

        RpcSpawnCop(position);
    }

    [ClientRpc]
    public void RpcSpawnCop(Vector3 position) {

    }


    // We can't disable the whole object, as it would impair synchronisation/communication
    // So disabling mean disabling collider & renderer only
    public void EnableCar(bool enable) {
        GetComponent<Collider>().enabled = enable;
        GetComponent<NetworkTransform>().enabled = enable;
        GetComponent<CarController>().enabled = enable;
        GetComponent<Rigidbody>().useGravity = enable;

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            renderer.enabled = enable;
        }

    }


    public void SetType (string newType) {
        Type = newType;
        EnableCar(Type == "CAR");
    }

    void OnDestroy()
    {
        GameManager.Players.Remove(this);
    }
}
