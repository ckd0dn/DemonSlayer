using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintWhite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material whiteMaterial;
    public Material outlineMaterial;
    private Coroutine flashWhiteCor;
    [SerializeField] private float duration;   
    [SerializeField] private float changeRatio;   

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlashWhite()
    {
        flashWhiteCor = StartCoroutine(FlashWhiteCoroutine());
    }

    private IEnumerator FlashWhiteCoroutine()
    {
        // 메테리얼을 whiteMaterial로 바꿈
        spriteRenderer.material = whiteMaterial;

        for (float flashAmount = duration; flashAmount > 0; flashAmount -= Time.deltaTime * changeRatio)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", flashAmount);
            yield return null;
        }

        spriteRenderer.material.SetFloat("_FlashAmount", 0);

        // 완전히 돌아오면 outlineMaterial로 바꿈
        spriteRenderer.material = outlineMaterial;

        StopCoroutine(flashWhiteCor);
    }
}