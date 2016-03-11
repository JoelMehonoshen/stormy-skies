using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BlackHole : MonoBehaviour {
	public List <GameObject> everything= new List<GameObject>();
	public float count;
	public float pullSpeed;
	public float distanceAway;
	public float maxDistAway;
	// Use this for initialization
	void Start () {
		distanceAway = 0;
		count = 0;
		everything.AddRange (GameObject.FindGameObjectsWithTag("Drop"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Lucky"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Player"));
		everything.AddRange (GameObject.FindGameObjectsWithTag("Hazard"));
	}

	// Update is called once per frame
	void Update () {


		foreach (GameObject thing in everything) {
			distanceAway = Vector3.Distance (thing.transform.position, transform.position);
			float step =  pullSpeed *Time.deltaTime;
			if (distanceAway >= maxDistAway)
			{
			} else thing.transform.position = Vector3.MoveTowards(thing.transform.position,transform.position,step);
		
	
		count = everything.Count;

	}
}
}