using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	public List<Texture2D> list;
	static UIManager instance;

	void Start(){
		instance = this;
		updateCoin ();
		int temp = PlayerPrefs.GetInt ("Level") % LevelManager.levelChange;
		levelText.text = "Level " + ((temp == 0 && PlayerPrefs.GetInt ("Level") > 0) ? 7 : temp);
		Texture2D skin = list.Find (item => item.GetInstanceID() == PlayerPrefs.GetInt("SkinID")-2);
		PlayerMovement.player.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(skin, new Rect(0,0,skin.width, skin.height), new Vector2(0.5f,0.5f));
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
		PlayerMovement.player.GetComponent<SpriteRenderer> ().sprite = selectedSkin;//update local(gameobject) skin
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") - price);//reduce money
		PlayerPrefs.SetInt ("SkinID", selectedSkin.GetInstanceID());//reduce money
		updateCoin();
		toggleShopConfirmPage (null);//close confirm page
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

