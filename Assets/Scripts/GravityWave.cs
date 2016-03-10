using UnityEngine;
using System.Collections;

public class GravityWave : MonoBehaviour {
	public Rigidbody2D playerRB; 


	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce (transform.right * -200);
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		//if (other.gameObject = 
		playerRB.AddForce(transform.right * -500); 



	}

	// Update is called once per frame
	void Update () {

	}
}
