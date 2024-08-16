using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class DamageTextUI : MonoBehaviour
{
    public Camera mainCamera;

    private WaitForSeconds TextDestroyTime = new WaitForSeconds(3);

    public void Init(Text damageTxt, Vector3 pos, float amount, Color color, float size)
    {
        damageTxt.transform.SetParent(UIManager.Instance.worldCanvas.transform , false);
        damageTxt.transform.position = pos;
        damageTxt.text = Mathf.Abs(Mathf.RoundToInt(amount)).ToString();
        damageTxt.color = color;
        StartCoroutine(TextMover(damageTxt.gameObject, damageTxt, pos));
    }

    private IEnumerator TextMover(GameObject damageTxtOb, Text damageTxt, Vector3 pos)
    {
        damageTxt.transform.DOMove(new Vector3(pos.x + 1f, pos.y + 1f, pos.z), 2f).SetEase(Ease.OutSine);
        damageTxt.DOFade(0f, 2f);
        yield return TextDestroyTime;
        damageTxtOb.SetActive(false);
    }

}
