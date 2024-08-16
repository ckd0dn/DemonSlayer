using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class IntroSceneUI : MonoBehaviour
{
    public TimelineSkipController timelineSkipController;
    
    public GameObject img;
    public GameObject whiteImg;
    public Text text;
    public GameObject TextPanel;
    private Image Image;
    private Image whiteImage;
    private string startText;
    private string firstDialog;
    private string secondDialog;
    private string thirdDialog;
    private string fourthDialog;
    private string fifthDialog;
    private string sixthDialog;
    private string sevenDialog;
    private WaitForSeconds waitTextDelay = new WaitForSeconds(2f);
    private WaitForSeconds waitText4Delay = new WaitForSeconds(4f);

    private void Start()
    {
        whiteImage = whiteImg.GetComponent<Image>();
        Image = img.GetComponent<Image>();
        startText = "....";
        firstDialog = "여긴... 어디지...";
        secondDialog = "저건...?";
        thirdDialog = "( 한번 가보자 )";
        fourthDialog = "왜 숲 속에 이런게 있지?..";
        fifthDialog = "뭔가 쓰여있다...";
        sixthDialog = "'힘을... 원한다면... 만져라...?'";
        sevenDialog = "( 뭔가에 홀린듯이 석상에 손을 가져간다. )";
        StartCoroutine(DialogEvent());
    }

    private void Update()
    {
        if(timelineSkipController.skipToPlay)
        {
            StopCoroutine(DialogEvent());
            SceneSkip();
        }
    }

    public IEnumerator DialogEvent()
    {
        yield return new WaitForSeconds(1f);
        TextPanel.SetActive(true);
        text.text = startText;
        yield return new WaitForSeconds(4f);
        ClearText();
        text.DOText(firstDialog, 2f);
        yield return waitTextDelay;
        ClearText();
        text.DOText(secondDialog, 2f);
        yield return waitTextDelay;
        ClearText();
        text.DOText(thirdDialog, 2f);
        yield return new WaitForSeconds(6f);
        ClearText();
        text.DOText(fourthDialog, 3f);
        yield return waitText4Delay;
        ClearText();
        text.DOText(fifthDialog, 3f);
        yield return waitText4Delay;
        ClearText();
        text.DOText(sixthDialog, 3f);
        yield return waitText4Delay;
        ClearText();
        text.DOText(sevenDialog, 3f);
        yield return waitText4Delay;
        TextPanel.SetActive(false);
        StartCoroutine(Fade(false));
        yield return waitText4Delay;
        StartCoroutine(WhiteFade());
        yield return waitText4Delay;
        LoadingManager.LoadScene("GameScene");
        //SceneManager.LoadScene("GameScene");
    }

    private void SceneSkip()
    {
        LoadingManager.LoadScene("GameScene");
        //SceneManager.LoadScene("GameScene");
    }

    private void ClearText()
    {
        text.text = "";
    }

    private IEnumerator Fade(bool isFadein)
    {
        if (isFadein)
        {
            Image.color = new Color(1, 1, 1, 1);
            Tween tween = Image.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
            img.SetActive(false);
        }
        else
        {
            Image.color = new Color(1, 1, 1, 0);
            img.SetActive(true);
            Tween tween = Image.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
        }
    }

    private IEnumerator WhiteFade()
    {
        Image.color = new Color(1, 1, 1, 0);
        whiteImg.SetActive(true);
        Tween tween = whiteImage.DOFade(1f, 1f);
        yield return tween.WaitForCompletion();
    }
}
