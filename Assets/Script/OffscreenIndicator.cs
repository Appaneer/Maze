using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour {

	/*
		If the maze is so large, we focus the camera at the player rather than position it at the center of the maze
	*/

    private GameObject target;
    public GameObject player;
    public Image arrow;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Target");
    }

    // Update is called once per frame
    void Update () {
        if (!target.GetComponent<SpriteRenderer>().isVisible)
            isTargetOnScreen();
        else
            arrow.enabled = false;
	}

    void isTargetOnScreen() {
        float rotation = Mathf.Atan2(target.transform.position.y - player.transform.position.y, target.transform.position.x - player.transform.position.x);//return in radian
        rotation = rotation * (180 / Mathf.PI); //convert radians to degrees
        arrow.enabled = true;
        arrow.GetComponent<RectTransform>().transform.localEulerAngles = new Vector3(0,0,rotation);
    }
}
