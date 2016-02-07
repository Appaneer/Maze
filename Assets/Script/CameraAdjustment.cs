using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraAdjustment : MonoBehaviour {

	/*
		This script position the camera at the center of the maze if the level is less than 15 
	*/

    public MazeGenerator mg;
	public float orthoZoomSpeed = 0.1f; // The rate of change of the orthographic size in orthographic mode.
	public float cameraSize;// the orthographic size of the main camera
	// Use this for initialization
	void Awake () {
        Camera cam = Camera.main;//find the main camer
        if ((mg.ySize - 10) / 2 < 15)// if the level is less than 15
        {
			cameraSize = mg.ySize / 2 + 1f;
			cam.orthographicSize = cameraSize;
			cam.transform.position = new Vector3(0f, (mg.ySize % 2 == 0) ? -0.5f : 0.0f, -10f);//position the camera at the center of the maze
        }
        else
        {
            cam.transform.position = new Vector3(0f, 0f, -10f);//else focus the camera at the player GameObject
            cam.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
            cam.orthographicSize = 40 / 2 - 7f;
        }
	}
		
}
