using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : InteractableObject
{
    public GameObject moverObj;
    private float moverObjHeight;
    public float duration = 3.0f; // 이동에 걸리는 시간
    public bool isUp = false;

    private CinemachineVirtualCamera virtualCamera;
    private Coroutine moveCor;

    public AudioClip clip;

    private Animator animator;

    private WaitForSeconds waitForSeconds;


    private void Awake()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        Vector3 virtualCameraPos = new Vector3 (moverObj.transform.position.x, moverObj.transform.position.y, -10);
        virtualCamera.transform.position = virtualCameraPos;

        moverObjHeight = moverObj.GetComponent<Renderer>().bounds.size.y;

        animator = GetComponent<Animator>();

        waitForSeconds = new WaitForSeconds(1f);
    }

    public override void Interact()
    {
        Execute();
    }

    private void Execute()
    {
        animator.SetTrigger("Press");

        moveCor = StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        yield return waitForSeconds;

        // 카메라 이동
        virtualCamera.Priority = 20;

        yield return waitForSeconds;

        // 사운드
        SoundManager.Instance.PlaySFX(clip);

        // 시작 위치와 목표 위치 설정
        Vector3 startPosition = moverObj.transform.position;

        float targetPositionY = isUp ? moverObj.transform.position.y + moverObjHeight : moverObj.transform.position.y - moverObjHeight;
        Vector3 targetPosition = new Vector3(moverObj.transform.position.x, targetPositionY, moverObj.transform.position.z);

        // 점진적 이동
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            moverObj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 설정
        moverObj.transform.position = targetPosition;

        // 카메라 원위치
        virtualCamera.Priority = 0;
        
        // 상호작용 끄기
        canInteract = false;

        HideInteractText();
    }
}
