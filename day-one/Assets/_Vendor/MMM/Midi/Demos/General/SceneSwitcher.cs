﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    public KeyCode triggerNextScene = KeyCode.RightBracket;
    public KeyCode triggerPreviousScene = KeyCode.LeftBracket;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered


    void Start() {
        
        // Destroy duplicates - there can be only one!
        foreach (var item in FindObjectsOfType<SceneSwitcher>())
        {
            if (item.gameObject != gameObject) Destroy(item.gameObject);
        }

        DontDestroyOnLoad(this);
        dragDistance = Screen.height * 0.1f; //dragDistance is 10% height of the screen
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerNextScene))
        {
            AdvanceBy(1);
        }

        if (Input.GetKeyDown(triggerPreviousScene))
        {
            AdvanceBy(-1);
        }

        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            AdvanceBy(1);
                        }
                        else
                        {   //Left swipe
                            AdvanceBy(-1);
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                        }
                        else
                        {   //Down swipe
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                }
            }
        }
    }

    void AdvanceBy(int amount)
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + amount;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        } else if (nextScene < 0)
        {
            nextScene = SceneManager.sceneCountInBuildSettings - 1;
        }
        SceneManager.LoadScene(nextScene);
    }
}