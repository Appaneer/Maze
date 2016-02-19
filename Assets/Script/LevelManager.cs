using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	/*
		This script manages both level and screen fading
		Spawning/instantiating chaser(s) and attaching torch on player are handled here
		When the player reaches the target, the boolean isFinishingLevel is called from TargetManager.cs
		When a chaser reaches the player,  the boolean isChased is called(TBD from which script)
		float fadeSpeed can be edited via inspector 
	*/

	public static int levelChange = 7;
	public MazeGenerator mg;
	public GameObject chaser;
	public GUITexture fader;
	[Range(0,3)]
	public float fadeSpeed; // how quickly the screen fades in and out
	private bool isStarting = true;
	public bool isFinishingLevel = false;
	public bool isChased = false;
	private Color faderColor;
	public Text levelText;

	void Start()
	{
		fader.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		faderColor = fader.color;
		TBD ();
	}

	void TBD(){
		switch (PlayerPrefs.GetInt("Level")) {
		case 0:
		case 1:
			break;
		case 2:
		case 3:
		case 4:
			Node startPos1 = mg.cells [mg.xSize - 1];//bottom right corner
			Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity);
			break;
		case 5:
		case 6:
		case 7:
			startPos1 = mg.cells [mg.xSize - 1];//bottom right corner
			Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity);
			Node startPos2 = mg.cells[mg.totalCells - mg.xSize];//top left corner
			Instantiate (chaser, new Vector2(startPos2.x, startPos2.y), Quaternion.identity);
			break;
		case 8:
		case 9:
			Camera.main.backgroundColor = Color.black;
			PlayerMovement.player.GetComponentInChildren<Light> ().enabled = true;
			break;
		case 10:
		case 11:
		case 12:
			Camera.main.backgroundColor = Color.black;
			PlayerMovement.player.GetComponentInChildren<Light> ().enabled = true;
			startPos1 = mg.cells [mg.xSize - 1];//bottom right corner
			GameObject tempChaser = Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity) as GameObject;
			tempChaser.GetComponentInChildren<Light> ().enabled = true;
			break;
		case 13:
		case 14:
		case 15:
			Camera.main.backgroundColor = Color.black;
			PlayerMovement.player.GetComponentInChildren<Light> ().enabled = true;
			startPos1 = mg.cells [mg.xSize - 1];//bottom right corner
			tempChaser = Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity) as GameObject;
			tempChaser.GetComponentInChildren<Light> ().enabled = true;
			startPos2 = mg.cells [mg.totalCells - mg.xSize];//top left corner
			GameObject tempChaser2 = Instantiate (chaser, new Vector2 (startPos2.x, startPos2.y), Quaternion.identity) as GameObject;
			tempChaser2.GetComponentInChildren<Light> ().enabled = true;
			break;
		}
	}

	void Update()
	{
		if (isStarting) 
			StartScene();
		
		if (isFinishingLevel)
			NextLevel ();
		else if (isChased)
			RestartLevel ();
	}


	void FadeToClear()
	{
		fader.color = Color.Lerp(fader.color, Color.clear, fadeSpeed * Time.deltaTime);
		levelText.color = Color.Lerp(levelText.color, Color.clear,  0.5f * Time.deltaTime);
		levelText.color = Color.Lerp(levelText.color, Color.clear,  0.5f * Time.deltaTime);
		levelText.color = Color.Lerp(levelText.color, Color.clear,  0.5f * Time.deltaTime);
		levelText.color = Color.Lerp(levelText.color, Color.clear,   Time.deltaTime);
	}


	void FadeToBlack()
	{
		fader.color = Color.Lerp(fader.color, faderColor, fadeSpeed * Time.deltaTime);
	}


	void StartScene()
	{
		FadeToClear();

		if (fader.color.a <= 0.05f)
		{
			fader.color = Color.clear;
			fader.enabled = false;
			isStarting = false;
		}
	}


	void NextLevel()
	{
		// Make sure the texture is enabled.
		fader.enabled = true;
		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (fader.color.a >= 0.8f) {
			if (PlayerPrefs.GetInt ("Level") == levelChange) {
				PlayerPrefs.SetInt("xSize", (int)(((double)Screen.width / Screen.height) * 10) - 2);//
				PlayerPrefs.SetInt("ySize", 10);
			} 
			else {
				PlayerPrefs.SetInt("xSize", PlayerPrefs.GetInt("xSize") + 2 * (Screen.width / Screen.height));
				PlayerPrefs.SetInt("ySize", PlayerPrefs.GetInt("ySize") + 2);
			}
			SceneManager.LoadScene("Maze");
			PlayerPrefs.SetInt ("Coins", ((PlayerPrefs.GetInt("Level") <= levelChange) ? PlayerPrefs.GetInt("Level") : (PlayerPrefs.GetInt("Level") % levelChange)) + PlayerPrefs.GetInt("Coins"));
			PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt("Level") + 1);
		}
	}

	public void Next(){
		if (PlayerPrefs.GetInt ("Level") == levelChange) {
			PlayerPrefs.SetInt("xSize", (int)(((double)Screen.width / Screen.height) * 10) - 2);//
			PlayerPrefs.SetInt("ySize", 10);
		} 
		else {
			PlayerPrefs.SetInt("xSize", PlayerPrefs.GetInt("xSize") + 2 * (Screen.width / Screen.height));
			PlayerPrefs.SetInt("ySize", PlayerPrefs.GetInt("ySize") + 2);
		}
		PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt("Level") + 1);
		SceneManager.LoadScene("Maze");
	}

	void RestartLevel()
	{
		// Make sure the texture is enabled.
		fader.enabled = true;
		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (fader.color.a >= 0.8f) {
			PlayerPrefs.SetInt("xSize", PlayerPrefs.GetInt("xSize") * (Screen.width / Screen.height));
			PlayerPrefs.SetInt("ySize", PlayerPrefs.GetInt("ySize"));
			SceneManager.LoadScene("Maze");
		}
	}

	public void Reset()
	{
		PlayerPrefs.SetInt ("Level", 0);
		PlayerPrefs.SetInt("xSize", (int)(((double)Screen.width / Screen.height) * 10) - 2 );
		PlayerPrefs.SetInt("ySize", 10);
	}


	public void Restart(){
		SceneManager.LoadScene("Maze");
	}
}