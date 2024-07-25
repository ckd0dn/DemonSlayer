using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    private float startTime;

    public float loadTime;
    public float playTime { get => data.playTime; set => data.playTime = value; }

    public Datas data;

    public void ResetTime()
    {
        startTime = Time.time;
        playTime = 0;
    }

    public void CheckTime()
    {
        playTime = loadTime + Time.time - startTime;
    }

    // 플레이 타임을 "00시 00분 00초" 형식의 문자열로 변환하는 메서드
    public string GetFormattedPlayTime(float time)
    {
        int hours = (int)(time / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);

        return $"{hours:D2}시 {minutes:D2}분 {seconds:D2}초";
    }
}
