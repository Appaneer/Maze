using UnityEngine;
using System.Collections.Generic;
using System;

public class ChaserManager : MonoBehaviour {

	/*
		This script is attached to each chaser
		The chaser(s) will be spawned/instantiated by LevelManager.cs
	*/

	MazeGenerator mg;
	GameObject player;
	Transform point;// it is a reference point for finding which node the player/chaser is currently on
	List<Node> path;
	[Range(0.1f, 0.01f)]
	public float speed;
	private int currentNode;
	public  bool isPaused = false;

	// Use this for initialization
	void Start()
	{
		mg = GameObject.Find ("Scripts").GetComponent<MazeGenerator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		point = GameObject.Find ("ref point").transform;
		currentNode = 0;
		path = mg.Astar(getNodePosition(transform.position), 0);
		point.position = new Vector2( mg.cells [0].x, mg.cells[0].y);
	}

	void Update() {
		if (isPaused) {
		}
		else if (path != null && !isPaused) {
			int currCell = 0;
			while (currCell < path.Count - 1) {// this while loop draw the path on console(scene view). Doesnt affect game view because it's debug
				Vector2 start = new Vector2 (path [currCell].x, path [currCell].y);
				Vector2 end = new Vector2 (path [currCell + 1].x, path [currCell + 1].y);
				Debug.DrawLine (start, end, Color.red);
				currCell++;
			}

			Vector2 tempNode = new Vector2 (path [currentNode].x, path [currentNode].y);
			transform.position = Vector2.Lerp (transform.position, tempNode, speed);
			if (Vector2.Distance (transform.position, tempNode) <= 0.05f) {
				currentNode++;//if the distance between current node and chaser's position is less than 0.05 then move on to the next node
			}
		} 

		path = mg.Astar(getNodePosition(transform.position), getNodePosition(player.transform.position));
	}

	int getNodePosition(Vector2 worldPos){
		int x = (int)(Mathf.Abs (worldPos.x - point.position.x)+0.5); 
		int y = (int)(Mathf.Abs (worldPos.y - point.position.y)+0.5);
		return (x + y * mg.xSize);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player"))
			GameObject.FindGameObjectWithTag("Scripts").GetComponent<LevelManager>().isChased = true;
	}

}
