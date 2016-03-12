using UnityEngine;
using System.Collections;

public class GravityWave : MonoBehaviour {
	public float waveSpeed;
	public Rigidbody2D thisWaveRB;
	public Rigidbody2D playerRB;
	public float pushPower;
	public Vector3 randomDirection;
	public float timer;
	public Vector3 originalPos;
	public float angle;
	// Use this for initialization
	void Start () {
		pushPower = Random.Range (5f, 10f);
		originalPos = transform.position;
		gameObject.SetActive (true);
		randomDirection = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), 0);
		waveSpeed = Random.Range (0.5f,3f);
		playerRB = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
		thisWaveRB = GetComponent<Rigidbody2D> ();
	}
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Debug.Log ("triggered");
			playerRB.AddForce (randomDirection * waveSpeed* pushPower);
		}
	}
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			gameObject.SetActive (false);
		}

		Vector3 moveDirection = gameObject.transform.position - originalPos;
		if (moveDirection != Vector3.zero) {
			angle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle+90f, Vector3.forward);
		}

		thisWaveRB.AddForce(randomDirection*waveSpeed);



	}
}
