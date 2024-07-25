using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rigidBody = GetComponent<Rigidbody2D>();
        // 실제 실행 주체는 healthSystem임
        healthSystem.OnDeath += OnDeath;
    }

    // 커스텀 해서 수정가능
    void OnDeath()
    {
        // 멈추도록 수정
        rigidBody.velocity = Vector3.zero;

        // 약간 반투명한 느낌으로 변경
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 2초뒤에 파괴
        Destroy(gameObject, 2f);
    }
}
