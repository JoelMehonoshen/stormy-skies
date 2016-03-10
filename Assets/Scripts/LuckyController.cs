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
	// Use this for initialization

	void Start () {
		luckyTransform = transform;
		gotTarget = false;
		target = playerTransform;
	}
	
	// Update is called once per frame
	void Update () {
		

		distanceToTarget = Vector3.Distance (click, luckyTransform.position);


		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
		if (Input.GetMouseButtonDown (0)&&!hit&&gotTarget==false) {
			click = Camera.main.ScreenToWorldPoint (Input.mousePosition);
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