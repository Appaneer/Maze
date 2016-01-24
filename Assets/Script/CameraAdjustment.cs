using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraAdjustment : MonoBehaviour {

	/*
		This script position the camera at the center of the maze if the level is less than 15 
	*/

    public MazeGenerator mg;
    public Text txt;
	// Use this for initialization
	void Awake () {
        Camera cam = Camera.main;//find the main camer
        if ((mg.ySize - 10) / 2 < 15)// if the level is less than 15
        {
            cam.orthographicSize = mg.ySize / 2 + 1f;//position the camera at the center of the maze
            cam.transform.position = new Vector3(0f, (mg.ySize % 2 == 0) ? -0.5f : 0.0f, -10f);
        }
        else
        {
            cam.transform.position = new Vector3(0f, 0f, -10f);//else focus the camera at the player GameObject
            cam.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
            cam.orthographicSize = 40 / 2 - 7f;
        }
	}

    void Start() {
        txt.text = "Level: "+((mg.ySize - 10) / 2);
    }
}
