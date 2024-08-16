using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : UIBase
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public TextMeshProUGUI endText;

    private void Awake()
    {
        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }
        fadeImage.color = new Color(0, 0, 0, 0); // 시작 시 투명
    }

    public void PlayerDie()
    {
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(fadeDuration * 1.5f);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(fadeDuration * 2);
        GameManager.Instance.Player.Respawn();
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(fadeDuration * 2);
        if(UIManager.Instance.dieUI != null)
        {
            Destroy(UIManager.Instance.dieUI.gameObject);
        }
    }

    public IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            if (endText != null)
            {
                endText.gameObject.SetActive(true);
            }
            fadeImage.color = new Color(0, 0, 0, alpha);
            timer += Time.unscaledDeltaTime; // deltaTime 사용 시, pasue에서 코루틴이 제대로 돌지않음
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeIn()
    {
        float timer = 0f;
        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }
        while (timer <= fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timer += Time.unscaledDeltaTime; 
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}
