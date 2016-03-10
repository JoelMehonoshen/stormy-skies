using UnityEngine;
using System.Collections;

public class AsteroidSpawn : MonoBehaviour {
	public GameObject asteroid; 


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

		if (Random.Range (1, 100) == 5) 
		{
			

			Instantiate (asteroid, transform.position, Quaternion.identity); 

		}	
	}
}
