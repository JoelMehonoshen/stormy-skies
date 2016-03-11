using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	public Vector3 spawnPos;

	public Transform left;
	public Transform right;
	public Transform up;
	public Transform down;
	public float timer;
	public float frequency;
	public GameObject wave;
	// Use this for initialization
	void Start () {
		timer = frequency;
	}
	
	// Update is called once per frame
	void Update () {
		spawnPos = new Vector3(Random.Range(left.transform.position.x,right.transform.position.x),Random.Range(up.transform.position.y,down.transform.position.y),0);
		timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject other = Instantiate (wave,spawnPos, Quaternion.identity) as GameObject;			
			timer = frequency;
		}

	}
}
