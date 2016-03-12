using UnityEngine;
using System.Collections;

public class NewAsteroidController : MonoBehaviour {
	public Vector3 randomDirection;
	public float randomSpeed;
	public float speed;
	public float timer;
	public Rigidbody2D thisRB;
	public GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		randomDirection = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), 0);
		gameObject.SetActive (true);
		thisRB = GetComponent<Rigidbody2D>();
		randomSpeed = Random.Range (0.1f, 3f);
		speed = randomSpeed;
	}
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			player.GetComponent<PlayerController> ().asteroidHit ();
		}
	}
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer<=0)
		{
			gameObject.SetActive (false);
		}
		thisRB.AddRelativeForce (randomDirection * speed);
	}
}
