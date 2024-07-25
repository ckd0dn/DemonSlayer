using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private Vector3 previousPosition; // 이전 프레임의 카메라 위치
    private Vector3 cameraMoveDir;   // 이동 벡터
    private float cameraMoveSpeed;   // 이동 벡터
    private Camera mainCamera;

    // [SerializeField]
	// private	Transform	target;				// 현재 배경과 이어지는 배경
	// [SerializeField]
	// private	float		scrollAmount;		// 이어지는 두 배경 사이의 거리
	[SerializeField]
	private	float moveSpeed;			// 배경 이동 속도
	private	float baseSpeed = 0.001f;			// 배경 이동 속도


    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        // 초기 위치 설정
        previousPosition = mainCamera.transform.position;
    }

    private void CalculateCameraDirAndSpeed()
    {
        // 현재 위치와 이전 위치의 차이를 이동 벡터로 계산
        cameraMoveDir = mainCamera.transform.position - previousPosition;
        cameraMoveDir.y = 0;
        cameraMoveDir = cameraMoveDir.normalized;

        // 이동 속도 계산 (단위: 유닛/초)
        cameraMoveSpeed = cameraMoveDir.magnitude / Time.deltaTime;

        // 현재 위치를 이전 위치로 업데이트
        previousPosition = mainCamera.transform.position;
    }

    private void FixedUpdate()
    {
        CalculateCameraDirAndSpeed();
        // 배경이 moveDirection 방향으로 moveSpeed의 속도로 이동
        transform.position += -cameraMoveDir * cameraMoveSpeed * moveSpeed * Time.deltaTime * baseSpeed;

        // 배경이 설정된 범위를 벗어나면 위치 재설정
        //if ( transform.position.x <= -scrollAmount )
        //{
        //	transform.position = target.position - cameraMoveDir * scrollAmount;
        //}
    }
}

