using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public Rigidbody2D playerRB;
	public float speed;
	public float turnSpeed;
	public float maxSpeed;
	public float currentSpeed;
	// Use this for initialization
	void Start () {
		playerRB = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentSpeed = playerRB.velocity.magnitude;
		if (Input.GetKey (KeyCode.W)) {
			playerRB.AddForce( transform.up * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.S)) {
			playerRB.AddForce( transform.up * Time.deltaTime * -speed);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.back * Time.deltaTime*turnSpeed);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.forward * Time.deltaTime*turnSpeed);
		}
		if (currentSpeed > maxSpeed) {
			playerRB.velocity = playerRB.velocity.normalized * maxSpeed;
		}

	}
}
