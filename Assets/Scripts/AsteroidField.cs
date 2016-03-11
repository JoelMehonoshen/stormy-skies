using UnityEngine;
using System.Collections;

public class AsteroidField : MonoBehaviour {
	private PlayerController player; 
	public Transform theplayer;
	public bool looking; 


	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerController> (); 
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (player) 
		{
			transform.LookAt (theplayer);
		}


	}
	
	// Update is called once per frame
	void Update () {
	


	}
}
