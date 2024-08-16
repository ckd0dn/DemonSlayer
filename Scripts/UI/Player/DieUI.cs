using UnityEngine;
using UnityEngine.UI;

public class DieUI : UIBase
{
    public FadeEffect fadeEffect;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<FadeEffect>();
    }
}
