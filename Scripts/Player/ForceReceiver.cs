using System.Collections;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody;

    [SerializeField] private float drag = 1f;

    private float verticalVelocity;

    private Player player;
    public GameObject playerEffect;
    public SpriteRenderer playerEffectRenderer;

    private WaitForSeconds doubleJumpEffectorDuration = new WaitForSeconds(0.3f);
    private WaitForSeconds doubleJumpEffectFadeOutTime = new WaitForSeconds(0.05f);
    public Vector2 Movement => impact + Vector2.up * verticalVelocity;
    private Vector2 dampingVelocity;
    private Vector2 impact;

    private void Start()
    {
        player = GetComponent<Player>();
        Rigidbody = GetComponent<Rigidbody2D>();
        playerEffectRenderer = playerEffect.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity, drag);
    }

    public void Reset()
    {
        verticalVelocity = 0f;
        impact = Vector2.zero;
    }

    public void AddForce(Vector2 force)
    {
        impact += force;
    }    

    public void Jump(float jumpForce)
    {
        if (player.isGrounded == true)
        {
            verticalVelocity += jumpForce;
        
            Vector2 velocity = Rigidbody.velocity;
            velocity.y = jumpForce;

            Rigidbody.velocity = velocity;

            player.isGrounded = false;
            player.isJumped = true;
        }
    }

    public void DoubleJump(float jumpForce)
    {
        StartCoroutine(DoubleJumpEffector());
        if (player.isJumped && player.DoubleJumpGet)
        {
            verticalVelocity += jumpForce;

            Vector2 velocity = Rigidbody.velocity;
            velocity.y = jumpForce;

            Rigidbody.velocity = velocity;
        }
    }

    IEnumerator DoubleJumpEffector()
    {
        if(player.isJumped && player.DoubleJumpGet)
        {
            playerEffect.SetActive(true);

            yield return doubleJumpEffectorDuration;
            for(float alpha  = 1.0f; alpha <= 0.0f;)
            {
                alpha -= 0.05f;
                playerEffectRenderer.color = new Color(0, 0, 0, alpha);
                yield return doubleJumpEffectFadeOutTime;
            }
            playerEffect.SetActive(false);
        }
    }
}