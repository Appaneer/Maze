using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Text creditText;
	public Text coinText;
	public Text levelText;
	public Canvas shopCanvas;
	public RectTransform settingPanel;
	public Animator anim;
	static UIManager instance;

	void Start(){
		instance = this;
		updateCoin ();
		levelText.text = "Level " + PlayerPrefs.GetInt ("Level");
	}

	public void startGame(){
		SceneManager.LoadScene ("maze");
	}

	public void toggleCreditPage(){
		creditText.enabled = !creditText.enabled;
	}

	public void pause(){
		GameObject[] cm = GameObject.FindGameObjectsWithTag ("Chaser");
		bool temp;

		if(PlayerPrefs.GetInt("Level") < 2)//there is no chasers at level 0 and 1
			temp = false;
		else
			temp = cm[0].GetComponent<ChaserManager> ().isPaused;
		
		foreach(GameObject c in cm){
			c.GetComponent<ChaserManager> ().isPaused = !temp;
		}
		Time.timeScale = (Time.timeScale != 0) ? 0 : 1;
	}

	public void clickedSetting(){
		settingPanel.gameObject.SetActive (true);
		anim.SetTrigger ("Clicked");
	}

	public static void updateCoin(){
		instance.coinText.text = "$" + PlayerPrefs.GetInt ("Coins");
	}
}
