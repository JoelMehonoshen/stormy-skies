using UnityEngine;
using System.Collections;

public class HazardSpawner : MonoBehaviour {
	public Vector3 spawnPos;
	public Transform left;
	public Transform up;
	public Transform right;
	public Transform  down;
	public GameObject [] hazards;
	public float timer;
	public float frequency;
	// Use this for initialization
	void Start () {
		timer = frequency;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject hazard = hazards[Random.Range(0,hazards.Length)];
		spawnPos = new Vector3(Random.Range(left.transform.position.x,right.transform.position.x),Random.Range(up.transform.position.y,down.transform.position.y),0);
		timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject other = Instantiate (hazard,spawnPos, Quaternion.identity) as GameObject;			
			timer = frequency;
		}
	}
}
