using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	/*
		This script manages both level and screen fading
		Spawning/instantiating chaser(s) and attaching torch on player are handled here
		When the player reaches the target, the boolean isFinishingLevel is called from TargetManager.cs
		When a chaser reaches the player,  the boolean isChased is called(TBD from which script)
		float fadeSpeed can be edited via inspector 
	*/
	public MazeGenerator mg;
	public GameObject chaser;
	public GUITexture fader;
	[Range(0,3)]
	public float fadeSpeed; // how quickly the screen fades in and out
	private bool isStarting = true;
	[HideInInspector]
	public bool isFinishingLevel = false;
	[HideInInspector]
	public bool isChased = false;
	private Color faderColor;

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
		case 5:
			Node startPos1 = mg.cells [GenerateChaserPos (true)];
			Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity);
			break;
		case 6:
		case 7:
		case 8:
			startPos1 = mg.cells [GenerateChaserPos (true)];
			Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity);
			Node startPos2 = mg.cells[GenerateChaserPos (false)];
			Instantiate (chaser, new Vector2(startPos2.x, startPos2.y), Quaternion.identity);
			break;
		case 9:
		case 10:
			Camera.main.backgroundColor = Color.black;
			GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Light> ().enabled = true;
			break;
		case 11:
		case 12:
			Camera.main.backgroundColor = Color.black;
			GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Light> ().enabled = true;
			startPos1 = mg.cells [GenerateChaserPos (true)];
			GameObject tempChaser = Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity) as GameObject;
			tempChaser.GetComponentInChildren<Light> ().enabled = true;
			break;
		case 13:
		case 14:
			Camera.main.backgroundColor = Color.black;
			GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Light> ().enabled = true;
			startPos1 = mg.cells [GenerateChaserPos (true)];
			tempChaser = Instantiate (chaser, new Vector2 (startPos1.x, startPos1.y), Quaternion.identity) as GameObject;
			tempChaser.GetComponentInChildren<Light> ().enabled = true;
			startPos2 = mg.cells [GenerateChaserPos (false)];
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
			if (PlayerPrefs.GetInt ("Level") == 8) {
				PlayerPrefs.SetInt("xSize", (int)(((double)Screen.width / Screen.height) * 10) - 2);//
				PlayerPrefs.SetInt("ySize", 10);
			} 
			else {
				PlayerPrefs.SetInt("xSize", PlayerPrefs.GetInt("xSize") + 2 * (Screen.width / Screen.height));
				PlayerPrefs.SetInt("ySize", PlayerPrefs.GetInt("ySize") + 2);
			}
			SceneManager.LoadScene("Maze");
			PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt("Level") + 1);
		}
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
		PlayerPrefs.SetInt("xSize", (int)(((double)Screen.width / Screen.height) * 10) - 2);//
		PlayerPrefs.SetInt("ySize", 10);
	}

	public void Restart(){
		SceneManager.LoadScene("Maze");
	}

	int GenerateChaserPos(bool TopOrBottom){//true = spawn the chaser at lower right, false = spawn the chaser at upper left
		int startCoordinate;
		int horSize = mg.xSize;
		int vertSize = mg.ySize;

		int randInt = Random.Range (0, 24);

		if (TopOrBottom) {
			if (randInt > 0 && randInt < 4) {
				startCoordinate = horSize - (randInt + 1);
			} else if (randInt > 5 && randInt < 9) {
				randInt %= 5;
				startCoordinate = (2 * horSize) - (randInt + 1);
			} else if (randInt > 10 && randInt < 14) {
				randInt %= 5;
				startCoordinate = (3 * horSize) - (randInt + 1);
			} else if (randInt > 15 && randInt < 19) {
				randInt %= 5;
				startCoordinate = (4 * horSize) - (randInt + 1);
			} else {
				randInt %= 5;
				startCoordinate = (5 * horSize) - (randInt + 1);
			}

			if (startCoordinate >= horSize * vertSize) 
				startCoordinate = horSize - 1;
			
		}
		else {
			if (randInt >= 0 && randInt <= 4) {
				startCoordinate = (vertSize - 5) * horSize + randInt;
			} else if (randInt >= 5 && randInt <= 9) {
				randInt %= 5;
				startCoordinate = (vertSize - 4) * horSize + randInt;
			} else if (randInt >= 10 && randInt <= 14) {
				randInt %= 5;
				startCoordinate = (vertSize - 3) * horSize + randInt;
			} else if (randInt >= 15 && randInt <= 19) {
				randInt %= 5;
				startCoordinate = (vertSize - 2) * horSize + randInt;
			} else {
				randInt %= 5;
				startCoordinate = (vertSize - 1) * horSize + randInt;
			}

			if (startCoordinate >= horSize * vertSize) 
				startCoordinate = horSize - 1;
		}
		return startCoordinate;
	}
}