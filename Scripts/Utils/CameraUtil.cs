using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraUtil : MonoBehaviour
{
    public float shakeIntensity = 1f;
    public float shakeTime = 0.2f;
    public float frequency = .1f;
    public float timeScale = .7f;

    public float timer;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin m_MultiChannelPerlin;

    void Start()
    {
        virtualCamera = GameManager.Instance.virtualCamera.GetComponent<CinemachineVirtualCamera>();
        m_MultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        m_MultiChannelPerlin.m_FrequencyGain = frequency;
    }

    public void Shake()
    {
        m_MultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
        Time.timeScale = timeScale; 
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        yield return new WaitForSeconds(shakeTime);

        m_MultiChannelPerlin.m_AmplitudeGain = 0f;
        Time.timeScale = 1f;
    }
}
