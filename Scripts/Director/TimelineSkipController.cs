using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSkipController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public double skipToTime = 28.0f;

    public GameObject skipTxt;

    public bool skipToPlay = false;
    public bool skipTxtOn = false;

    private void Update()
    {
        // 특정 상황 감지하는 조건
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipTxt.SetActive(true);
            skipTxtOn = true;
            if (skipTxtOn && Input.GetKeyDown(KeyCode.Space))
            {
                SkipTimeline();
                skipToPlay = true;
            }
        }
    }

    void SkipTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.time = skipToTime;
            playableDirector.Evaluate(); // Evaluate를 호출하여 타임라인 상태를 즉시 반영
        }
    }

}
