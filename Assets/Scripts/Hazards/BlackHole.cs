using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BlackHole : MonoBehaviour {
	public List <GameObject> everything= new List<GameObject>();
	public float count;
	public float pullSpeed;
	public float distanceAway;
	public float maxDistAway;
	public float timer;
	public GameObject player;
	public GameObject child;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		distanceAway = 0;
		count = 0;
		everything.AddRange (GameObject.FindGameObjectsWithTag("Drop"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Lucky"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Player"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Hazard"));
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			player.GetComponent<PlayerController> ().blackHole ();
		} else if (col.gameObject.tag == "Lucky") {} 
		else if (col.gameObject.tag == "Wall") {} 
		else
		col.gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;
		if (timer <= 0) {
			Destroy (child);
			Destroy (gameObject);
		}


		foreach (GameObject thing in everything) {
			if (thing != null) {
				distanceAway = Vector3.Distance (thing.transform.position, transform.position);
				float step = pullSpeed * Time.deltaTime;
				if (distanceAway >= maxDistAway) {
				} else
					thing.transform.position = Vector3.MoveTowards (thing.transform.position, transform.position, step);
		
	
				count = everything.Count;
			}
	}
}
}