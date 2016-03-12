using UnityEngine;
using System.Collections;

public class SolarRays : MonoBehaviour {
	public GameObject playerController;
	public Mesh startingMesh;
	public Mesh endMesh;
	public float timer;


	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter> ().mesh = startingMesh;
		playerController = GameObject.FindGameObjectWithTag ("Player");
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player" && timer <= 0 && timer >= -1) {

			playerController.GetComponent<PlayerController> ().hitWall ();
		}
	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			GetComponent<MeshFilter> ().mesh = endMesh;
		}
		if (timer <= -1) {
			Destroy (gameObject);
		}
	}
}
