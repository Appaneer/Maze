﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Advertisements;

public class UIManager : MonoBehaviour {

	public Text creditText;
	public Text coinText;
	public Text levelText;
	public Canvas shopCanvas;
	public Canvas shopConfirmPage;
	public RectTransform settingPanel;
	public Animator anim;
	private Sprite selectedSkin;
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

	public void openShop(){
		shopCanvas.enabled = !shopCanvas.enabled;
		//pause ();
	}

	public void toggleShopConfirmPage(Image flag){
		shopConfirmPage.enabled = !shopConfirmPage.enabled;
		selectedSkin = (flag == null) ? null : flag.sprite;
	}

	public void buy(int price){
		if (PlayerPrefs.GetInt ("Coins") < price) {//if the player has enough money to buy this 
			toggleShopConfirmPage (null);//close confirm page
			return;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<SpriteRenderer> ().sprite = selectedSkin;//update local(gameobject) skin
		GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");//get prefab
		player.GetComponent<SpriteRenderer> ().sprite = selectedSkin;//update prefab skin
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - price);//reduce money
		updateCoin();
	}

	public static void updateCoin(){
		instance.coinText.text = "$" + PlayerPrefs.GetInt ("Coins");
	}

	public void ShowRewardedAd(){
		if(Advertisement.IsReady("rewardedVideoZone") && Time.timeScale == 0){
			var options = new ShowOptions{ resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideoZone", options);
		}
	}

	private void HandleShowResult(ShowResult result){
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("Successfully shown");
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + 10);
			updateCoin ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("Ad skipped");
			break;
		case ShowResult.Failed:
			Debug.Log ("Ad failed to be shown");
			break;
		}
	}

}


/* I used this method to auto assign country flag to each button. It works perfectly
	 * Now we don't need this. Just to keep a copy in case we want to go back
	 * private void DoStuff(){
		Array.Sort (countryFlags,
			delegate(Sprite i1, Sprite i2) 
			{ 
				return i1.name.CompareTo(i2.name); 
			}
		);

		try
		{
			string line;
			StreamReader input = new StreamReader("Assets/names.txt", Encoding.Default);
			using (input)
			{
				do
				{
					line = input.ReadLine();
					if (line != null)
					{
						string[] list = line.Split(',');

						for(int i = 0; i < list.Length-1; i++){
							string countryName = list[i];
							buttonList[i].name = countryName;
							buttonList[i].image.sprite = countryFlags[i];
						}

					}
				}
				while (line != null);    
				input.Close();
			}
		}
		catch (FileNotFoundException e)
		{
			Debug.LogError ("FUCCCCCKKKKK");
		}
	}
	 * 
	*/