using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
	public float health;
	public float fuel;
	public float fuelConsumption;
	public GameObject gameOverScreen;
	public GameObject fuelSlider;
	public GameObject healthSlider;
	public GameObject player;
	public float outOfFuelTimer;
	public Text fuelText;
	public Text healthText;
	public GameObject pauseText;
	public bool isPaused;
	// Use this for initialization
	void Start () {
		outOfFuelTimer = 5;
		Time.timeScale = 1;
		fuelText.text = "";
		healthText.text = "";
		player = GameObject.FindGameObjectWithTag ("Player");
		gameOverScreen.SetActive (false);
		health = 100;
		fuel = 100;
		fuelSlider = GameObject.Find ("FuelSlider");
		healthSlider = GameObject.Find ("HealthSlider");
		Time.timeScale=1;
		pauseText.SetActive(false);
	}
	public void OnClickRestart(){
		Application.LoadLevel (1);
	}
	public void OnClickMainMenu(){
		Application.LoadLevel (0);
	}
	public void fuelDrop(){
		fuel += 10;
		Debug.Log ("fuel");
	}
	public void healthDrop(){
		health += 10;
	}
	public void outOfFuel(){
		gameOverScreen.SetActive (false);
	}
	public void hitWall(){
		health -=20;
	}
	public void GameOver(){
		gameOverScreen.SetActive (true);
		Time.timeScale = 0;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			if(isPaused){
				pauseText.SetActive(false);
				Time.timeScale=1;
				isPaused = false;
			}else if(!isPaused){
				pauseText.SetActive(true);
				Time.timeScale=0;
				isPaused = true;
			}
		}
		if (fuel > 100) {
			fuel = 100;
		}
		if (health > 100) {
			health = 100;
		}
		if (fuel <= 0) {
			fuel = 0;
			outOfFuelTimer -= Time.deltaTime;
			player.GetComponent<PlayerMovement> ().speed = 0;
		}else player.GetComponent<PlayerMovement> ().speed= 100;
		if (outOfFuelTimer <= 0) {
			GameOver ();
			fuelText.text = "Lost in the storm, you ran out of fuel and died a painful death...";
		}
		if (health <= 0) {
			GameOver ();
			healthText.text = "Your ships hull was comprimised and your head exploded.";
		}


		fuelSlider.GetComponent<Slider>().value = fuel;
		healthSlider.GetComponent<Slider>().value= health;
		if (Input.GetKey(KeyCode.W)) {
			fuel -= fuelConsumption * Time.deltaTime;
		}
	}
}
