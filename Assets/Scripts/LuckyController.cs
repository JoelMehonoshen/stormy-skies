using UnityEngine;
using System.Collections;

public class LuckyController : MonoBehaviour {
	public Transform target;
	public float smoothTime = 0.3f;
	private Transform luckyTransform;
	public Transform playerTransform;
	public Vector3 click;
	public bool gotTarget;
	public float distanceToTarget;
	public bool hasDrop;
	public GameObject drop;
	public float resetTimer;
	public float currentSpeed;
	public Rigidbody2D luckyRB;
	// Use this for initialization

	void Start () {
		resetTimer = 5;
		luckyTransform = transform;
		gotTarget = false;
		target = playerTransform;
		hasDrop = false;
		drop = null;
	}
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Drop"&&hasDrop==false) {
			drop = col.gameObject;
			drop.transform.parent = transform;
			hasDrop = true;
			gotTarget = false;
		}
		if (col.gameObject.tag == "Player" && hasDrop) {
			drop.transform.position = playerTransform.position;
			hasDrop = false;
		}
	}

	// Update is called once per frame
	void Update () {

		currentSpeed = luckyRB.velocity.magnitude;
		if (currentSpeed <= 0.1) {
			resetTimer-=Time.deltaTime;
			if (resetTimer<=0){
				gotTarget=false;
				resetTimer =5;
			}
		}

		distanceToTarget = Vector3.Distance (click, luckyTransform.position);


	RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
	
			
		
		if (Input.GetMouseButtonDown (0)&&hit&&gotTarget==false&&hit.collider.tag =="Drop") {

			//click = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			click = hit.collider.transform.position;
			click.z = 0;
			gotTarget = true;

		}
		if (gotTarget == true) {
			luckyTransform.position = Vector3.Lerp (transform.position, click, Time.deltaTime*smoothTime);
		}
		else
			luckyTransform.position = Vector3.Lerp (transform.position, target.position, Time.deltaTime*smoothTime);
		if (distanceToTarget <= 0.1) {
			gotTarget = false;
		}
			

}
}