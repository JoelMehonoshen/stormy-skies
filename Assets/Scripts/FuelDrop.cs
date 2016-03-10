using UnityEngine;
using System.Collections;

public class FuelDrop : MonoBehaviour {
	public GameObject player;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag== "Player") {
			player.GetComponent<PlayerController>().fuelDrop ();
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
	}
}
