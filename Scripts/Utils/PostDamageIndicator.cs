using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostDamageIndicator : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;

    private WaitForSeconds damageIndicatorDelay = new WaitForSeconds(0.5f);

    void Start()
    {
        GameManager.Instance.Player.healthSystem.OnDamage += ShowDamageEffect;
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    public void ShowDamageEffect()
    {
        vignette.active = true;
        vignette.color.value = Color.red;
        vignette.intensity.value = 0.5f;

        Invoke("HideDamageEffect", 0.5f);
    }

    void HideDamageEffect()
    {
        vignette.active = false;
    }

}
