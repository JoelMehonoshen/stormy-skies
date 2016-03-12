using UnityEngine;
using System.Collections;

public class NewAsteroidController : MonoBehaviour {
	public Vector3 randomDirection;
	public float randomSpeed;
	public float speed;
	public float timer;
	public Rigidbody2D thisRB;
	public GameObject player;
	public bool hit;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		randomDirection = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), 0);
		gameObject.SetActive (true);
		thisRB = GetComponent<Rigidbody2D>();
		randomSpeed = Random.Range (0.1f, 3f);
		speed = randomSpeed;
		hit = false;
		transform.localScale += new Vector3 (Random.Range (0f, 2f), Random.Range (0f, 2f),0f);
	}
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player"&&hit ==false) {
			player.GetComponent<PlayerController> ().asteroidHit ();
			hit = true;
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
