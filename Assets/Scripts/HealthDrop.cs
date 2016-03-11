using UnityEngine;
using System.Collections;

public class HealthDrop : MonoBehaviour {
	public GameObject player;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		gameObject.SetActive(true);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag== "Player") {
			player.GetComponent<PlayerController>().healthDrop ();
			gameObject.SetActive(false);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
