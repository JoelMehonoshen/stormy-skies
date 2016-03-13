using UnityEngine;
using System.Collections;

public class FuelDrop : MonoBehaviour {
	public GameObject player;
	public float timer;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		gameObject.SetActive(true);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag== "Player") {
			player.GetComponent<PlayerController>().fuelDrop ();
			gameObject.SetActive(false);
		}
		if (col.gameObject.tag == "Lucky") {
			timer = 99999;
		}
	}
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			gameObject.SetActive (false);
		}
	}
}
