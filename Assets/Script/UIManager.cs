using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Text creditText;
	public Text coinText;
	public Text levelText;
	public Canvas shopCanvas;

	void Start(){
		updateCoin ();
		levelText.text = "Level " + PlayerPrefs.GetInt ("Level");
	}

	void Update(){
		
	}

	public void startGame(){
		SceneManager.LoadScene ("maze");
	}

	public void toggleCreditPage(){
		creditText.enabled = !creditText.enabled;
	}

	public void pause(){
		GameObject[] cm = GameObject.FindGameObjectsWithTag ("Chaser");
		bool temp = cm[0].GetComponent<ChaserManager> ().isPaused;
		foreach(GameObject c in cm){
			c.GetComponent<ChaserManager> ().isPaused = !temp;
		}
		Time.timeScale = (Time.timeScale != 0) ? 0 : 1;
	}

	public void openShop(){

	}

	public void updateCoin(){
		coinText.text = "$" + PlayerPrefs.GetInt ("Coins");
	}
}
