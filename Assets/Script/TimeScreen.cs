using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class TimeScreen : MonoBehaviour
{
    public Text timeText;
    void Start()
    {

        float secondsSpentPlaying = PlayerPrefs.GetFloat("final time");

        TimeSpan time = TimeSpan.FromSeconds(secondsSpentPlaying);

        timeText.text = time.ToString("hh':'mm':'ss");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
