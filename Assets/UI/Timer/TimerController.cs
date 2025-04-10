using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text textComp;

    private float elapsedRunningTime = 0f;
    private float runningStartTime = 0f;
    private float pauseStartTime = 0f;
    private float elapsedPausedTime = 0f;
    private float totalElapsedPausedTime = 0f;
    private bool running = false;
    private bool paused = false;

    void Update()
    {
        if (running)
        {
            elapsedRunningTime = Time.time - runningStartTime - totalElapsedPausedTime;
            UpdateUI();
        }
        else if (paused)
        {
            elapsedPausedTime = Time.time - pauseStartTime;
        }

    }

    private void UpdateUI()
    {
        textComp.text = GetTime().ToString(@"mm\:ss\.fff");
    }

    public void Begin()
    {
        if (!running && !paused)
        {
            runningStartTime = Time.time;
            running = true;
        }
    }

    public void Pause()
    {
        if (running && !paused)
        {
            running = false;
            pauseStartTime = Time.time;
            paused = true;
        }
    }

    public void Unpause()
    {
        if (!running && paused)
        {
            totalElapsedPausedTime += elapsedPausedTime;
            running = true;
            paused = false;
        }
    }

    public void Reset()
    {
        elapsedRunningTime = 0f;
        runningStartTime = 0f;
        pauseStartTime = 0f;
        elapsedPausedTime = 0f;
        totalElapsedPausedTime = 0f;
        running = false;
        paused = false;
    }

    public int GetTimeSeconds()
    {
        return (int)elapsedRunningTime;
    }
    public TimeSpan GetTime()
    {
        return TimeSpan.FromSeconds(elapsedRunningTime);
    }
}
