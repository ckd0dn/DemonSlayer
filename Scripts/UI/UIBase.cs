using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField] private bool isPopUpSound;

    [SerializeField] private AudioClip popUpClip;
    public void OnUI()
    {
        gameObject.SetActive(true);
        if(isPopUpSound) 
            SoundManager.Instance.PlaySFX(popUpClip);
    }

    public void OffUI()
    {
        gameObject.SetActive(false);
    }
}