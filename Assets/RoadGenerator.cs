using UnityEngine;
using System.Collections;

public class RoadGenerator : MonoBehaviour {
    public GameObject Road;
    public int MinLenght = 5;
    public int MaxLength = 20;
    public int Size = 100;
    public float StepSize = 1;

    int x = 0, y = 0;
    Vector3 position = Vector3.zero;

    void Start () {
        for (int i = 0; i < Size; i++) {
            CreateStraightRoad();
            PickDirection();
        }
	}

    void CreateStraightRoad() {
        int length = Random.Range(MinLenght, MaxLength);
        for (int e = 0; e < length; e++) {
            GameObject instance = Instantiate<GameObject>(Road);
            Vector3 step = (Vector3.forward * y + Vector3.right * x) * StepSize;
            instance.transform.position = position + step;
            position = instance.transform.position;
        }
    }

    void PickDirection() {
        int ran = Random.Range(0, 2) * 2;
        if (x == 0) {
            x = 1 - ran;
            y = 0;
        } else {
            y = 1 - ran;
            x = 0;
        }
    }
}
