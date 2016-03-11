using UnityEngine;
using System.Collections;

public class GravityWave : MonoBehaviour {
	public float waveSpeed;
	public float waveRotation;
	public GameObject thisWave;
	// Use this for initialization
	void Start () {
		waveSpeed = 2.5f;
		transform.Rotate (Vector3.forward * waveRotation);
	}

	// Update is called once per frame
	void Update () {
		
		transform.Translate(Vector3.right*Time.deltaTime*waveSpeed);
	}
}
