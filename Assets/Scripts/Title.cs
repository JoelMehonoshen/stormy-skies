using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public void OnClickPlay(){
		Application.LoadLevel (1);
	}
	public void OnClickExit(){
		Application.Quit ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
