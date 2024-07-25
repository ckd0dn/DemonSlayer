using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintWhite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material whiteMaterial;
    private Coroutine flashWhiteCor;
    [SerializeField] private float duration;   
    [SerializeField] private float changeRatio;   

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.material = whiteMaterial;

    }

    public void FlashWhite()
    {
        flashWhiteCor = StartCoroutine(FlashWhiteCoroutine());
    }

    private IEnumerator FlashWhiteCoroutine()
    {

        for(float flashAmount = duration; flashAmount > 0; flashAmount -= Time.deltaTime * changeRatio)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", flashAmount);
            yield return null;
        }

        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        StopCoroutine(flashWhiteCor);
    }
}