using UnityEngine;
using System.Collections;

public class SpawnFood : MonoBehaviour {

	public GameObject prefabToSpawn;

	public Transform borderNorth;
	public Transform borderEast;
	public Transform borderSouth;
	public Transform borderWest;

	// Use this for initialization
	void Start() {
		Spawn();
	}

	void Spawn() {
		int x = (int)Random.Range (borderWest.position.x, borderEast.position.x);
		int y = (int)Random.Range (borderSouth.position.x, borderNorth.position.x);
		Instantiate (prefabToSpawn, new Vector2 (x, y), Quaternion.identity);
	}
}
