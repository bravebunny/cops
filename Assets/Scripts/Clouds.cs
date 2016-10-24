using UnityEngine;
using System.Collections;

public class Clouds : MonoBehaviour {
    public GameObject OriginalCloud;
    public int ParcelCountX = 10;
    public int ParcelCountZ = 10;
    public Vector3 ParcelBounds; // volume in which each cloud will be generated

	void Start () {
        Vector3 offset = new Vector3(ParcelCountX * ParcelBounds.x, 0, ParcelCountZ * ParcelBounds.z) / 2;
        for (int x = 0; x < ParcelCountX; x++) {
            for (int z = 0; z < ParcelCountZ; z++) {
                // parcel top left position
                float parcelX = x * ParcelBounds.x;
                float parcelZ = z * ParcelBounds.z;
                Vector3 parcelPosition = new Vector3(parcelX, 0, parcelZ) - offset;

                // generate a random position inside the parcel
                float cloudX = Random.Range(0, ParcelBounds.x);
                float cloudY = Random.Range(0, ParcelBounds.y);
                float cloudZ = Random.Range(0, ParcelBounds.z);
                Vector3 cloudPosition = parcelPosition + new Vector3(cloudX, cloudY, cloudZ);

                GameObject cloud = Instantiate<GameObject>(OriginalCloud);
                cloud.transform.parent = transform;
                cloud.transform.localPosition = cloudPosition;
            }
        }
	}
}
