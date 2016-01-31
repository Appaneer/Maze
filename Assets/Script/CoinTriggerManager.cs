using UnityEngine;
using System.Collections;

public class CoinTriggerManager : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals ("Player")) {
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt("Coins") + 1);
			Destroy (gameObject);
		}
		GameObject.Find ("Scripts").GetComponent<UIManager>().updateCoin();
	}
}
