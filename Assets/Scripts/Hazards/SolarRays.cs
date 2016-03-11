using UnityEngine;
using System.Collections;

public class SolarRays : MonoBehaviour {
	public GameObject playerController;


	// Use this for initialization
	void Start () {
		//playerController = GetComponent<PlayerController> ();
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {

			playerController.GetComponent<PlayerController> ().hitWall ();


		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
