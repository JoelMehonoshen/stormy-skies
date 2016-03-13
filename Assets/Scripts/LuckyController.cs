using UnityEngine;
using System.Collections;

public class LuckyController : MonoBehaviour {
	public Transform target;
	public float smoothTime = 0.3f;
	private Transform luckyTransform;
	public Transform playerTransform;
	public GameObject click;
	public bool gotTarget;
	public bool hasDrop;
	public GameObject drop;
	public Rigidbody2D luckyRB;
	public float distanceToPlayer;
	public bool canTarget;
	// Use this for initialization

	void Start () {
		canTarget = false;
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
			canTarget = false;
			gotTarget = false;
		}
		if (col.gameObject.tag == "Player" && hasDrop) {
			drop.transform.position = playerTransform.position;
			hasDrop = false;
			click = null;
		}
	}

	// Update is called once per frame
	void Update () {
		distanceToPlayer = Vector3.Distance (playerTransform.position, luckyTransform.position);
		if (distanceToPlayer >= 2.5) {
			smoothTime = 10;
		} else
			smoothTime = 3;
		
	RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
	
			
		
		if (Input.GetMouseButtonDown (0)&&hit&&gotTarget==false&&hit.collider.tag =="Drop") {
			//click = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			canTarget =true;
			click = hit.transform.gameObject;
			gotTarget = true;
		}
		if (canTarget == true&&click!=null) {
			
			click.transform.position = new Vector3 (click.transform.position.x, click.transform.position.y, 0f);
		}

		if (gotTarget == true&&click!=null) {
			luckyTransform.position = Vector3.MoveTowards (transform.position, click.transform.position, Time.deltaTime*smoothTime);
		}
		else
			luckyTransform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime*smoothTime);
			

}
}