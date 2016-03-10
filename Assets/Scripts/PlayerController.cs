using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
	public float health;
	public float fuel;
	public float fuelConsumption;

	public GameObject fuelSlider;
	public GameObject healthSlider;
	// Use this for initialization
	void Start () {
		health = 100;
		fuel = 100;
		fuelSlider = GameObject.Find ("FuelSlider");
		healthSlider = GameObject.Find ("HealthSlider");
	}

	public void fuelDrop(){
		fuel += 10;
		Debug.Log ("fuel");
	}
	public void healthDrop(){
		health += 10;
	}

	
	// Update is called once per frame
	void Update () {
		if (fuel > 100) {
			fuel = 100;
		}
		if (health > 100) {
			health = 100;
		}
		fuelSlider.GetComponent<Slider>().value = fuel;
		healthSlider.GetComponent<Slider>().value= health;
		if (Input.GetKey(KeyCode.W)) {
			fuel -= fuelConsumption * Time.deltaTime;
		}
	}
}
