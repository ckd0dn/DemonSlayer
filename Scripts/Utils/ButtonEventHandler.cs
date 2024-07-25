using DG.Tweening;
using UnityEngine;

public class ButtonEventHandler : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float pointDownSize = 0.9f;
    [SerializeField] private float pointUpSize = 1f;

    private Vector3 vectorDownSize;
    private Vector3 vectorUpSize;

    [SerializeField] private AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        vectorDownSize = new Vector3(pointDownSize, pointDownSize, pointDownSize);
        vectorUpSize = new Vector3(pointUpSize, pointUpSize, pointUpSize);
    }

    public void SizeDown()
    {
        rectTransform.DOScale(vectorDownSize, 0.2f);
    }

    public void SizeUp()
    {
        rectTransform.DOScale(vectorUpSize, 0.2f);
        ClickSound();
    }

    void ClickSound()
    {
        SoundManager.Instance.PlaySFX(audioClip);
    }
}
