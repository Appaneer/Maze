using UnityEngine;
using System.Collections;

public class CoinManager : MonoBehaviour {

	/*
	 * This script is responible for coin management
	 */



	// Use this for initialization
	void Start () {
		Debug.Log (PlayerPrefs.GetInt("Coins"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
