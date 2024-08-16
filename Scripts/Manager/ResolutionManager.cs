using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;

    private Resolution[] resolutions;
    private Resolution initialResolution = new Resolution { width = 1440, height = 900 };
    private bool initialFullscreenMode = false; 

    void Start()
    {
        // 모든 해상도 가져오기
        resolutions = Screen.resolutions;

        // 중복 해상도 제거
        resolutions = resolutions.Distinct(new ResolutionComparer()).ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == initialResolution.width &&
                resolutions[i].height == initialResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // 해상도 변경 이벤트 리스너 추가
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        fullscreenDropdown.ClearOptions();
        List<string> fullscreenOptions = new List<string> { "Fullscreen", "Windowed" };
        fullscreenDropdown.AddOptions(fullscreenOptions);
        fullscreenDropdown.value = initialFullscreenMode ? 0 : 1; // Windowed mode is selected by default
        fullscreenDropdown.RefreshShownValue();

        // 전체 화면 모드 변경 이벤트 리스너 추가
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreenMode);

        // 초기 설정 적용
        SetResolution(currentResolutionIndex);
        SetFullscreenMode(fullscreenDropdown.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreenMode(int fullscreenIndex)
    {
        bool isFullscreen = fullscreenIndex == 0; // 0 = Fullscreen, 1 = Windowed

        // 현재 전체 화면 모드와 새로 설정할 모드가 다를 때만 변경
        if (Screen.fullScreen != isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}

// 해상도를 비교하여 중복 제거하는 Comparer 클래스
public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}
