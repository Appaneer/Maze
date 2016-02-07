using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{ 
	/*
		This script manages player movement and it attaches to the player
	*/

    public float speed;
    private float h = 0;
	private float v = 0;
    private Rigidbody2D rb2d;
    public MazeGenerator mg;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;
    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        gameObject.transform.position = (mg.cells[0].north.transform.position + mg.cells[0].south.transform.position) / 2;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.A))
        {
            h = -1;
            v = 0;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            h = 1;
            v = 0;

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            h = 0;
            v = 1;

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            h = 0;
            v = -1;

        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {

            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:// a new touch begins
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled://the touch is cancelled
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:// the touch is ended so now we can calculate the time and distance

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    h = 1;
                                    v = 0;// swipe right
                                }
                                else {
                                    h = -1;
                                    v = 0;// swipe left
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    h = 0;//swipe up
                                    v = 1;
                                }
                                else {
                                    h = 0;//swipe down
                                    v = -1;
                                }
                            }

                        }
                        break;
                }
            }
        }

#endif

        rb2d.velocity = new Vector2(h * speed, v * speed);
    }
}
