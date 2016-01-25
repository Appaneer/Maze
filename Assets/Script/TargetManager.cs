using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.tag.Equals("Player"))//thia ia plyer
			GameObject.FindGameObjectWithTag("Scripts").GetComponent<LevelManager>().isFinishingLevel = true;
    }
}