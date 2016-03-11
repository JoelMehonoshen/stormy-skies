using UnityEngine;
using System.Collections;

public class Asteroid2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce (transform.right * 200); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
