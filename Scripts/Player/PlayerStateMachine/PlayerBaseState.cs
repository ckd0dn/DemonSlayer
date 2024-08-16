using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }
    protected virtual void AddInputActionCallbacks()
    {
        //if (UIManager.Instance.onUI) return;
        if (stateMachine.Player.isSkillActive) return;

        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Jump.started += OnJumpStarted;
        input.PlayerActions.Attack.performed += OnAttackPerformed;
        input.PlayerActions.Attack.canceled += OnAttackCanceled;
        input.PlayerActions.Dodge.started += OnDodgeStarted;
        input.PlayerActions.FirstSkill.started += OnFirstSkillStarted;
        input.PlayerActions.SecondSkill.started += OnSecondSkillStarted;
        input.PlayerActions.ThirdSkill.started += OnThirdSkillStarted;
        input.PlayerActions.SkillSlotSwap.started += OnSkillSlotSwap;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        input.PlayerActions.Jump.started -= OnJumpStarted;
        input.PlayerActions.Attack.performed -= OnAttackPerformed;
        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        input.PlayerActions.Dodge.started -= OnDodgeStarted;
        input.PlayerActions.FirstSkill.started -= OnFirstSkillStarted;
        input.PlayerActions.SecondSkill.started -= OnSecondSkillStarted;
        input.PlayerActions.ThirdSkill.started -= OnThirdSkillStarted;
        input.PlayerActions.SkillSlotSwap.started -= OnSkillSlotSwap;
    }

    public virtual void HandleInput()
    {
        //if (!UIManager.Instance.onUI)
            ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnFirstSkillStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnSecondSkillStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnThirdSkillStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnSkillSlotSwap(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.firstSkillSlot == true)
        {
            stateMachine.Player.firstSkillSlot = false;
            UIManager.Instance.ui.skillUI.SkillSlotMover();
        }
        else if (!stateMachine.Player.firstSkillSlot)
        {
            stateMachine.Player.firstSkillSlot = true;
            UIManager.Instance.ui.skillUI.SkillSlotMover();
        }
    }

    protected void StartTriggerAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetTrigger(animatorHash);
    }

    protected void StopTriggerAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.ResetTrigger(animatorHash);
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 movementDirection = stateMachine.MovementInput.normalized;

        Move(movementDirection);
    }

    private void Move(Vector2 direction)
    {
        float movementSpeed = GetMovementSpeed();

        Vector2 rb = stateMachine.Player.Rigidbody.velocity;
        rb.x = direction.x * movementSpeed;
        stateMachine.Player.Rigidbody.velocity = rb;
        if(!stateMachine.Player.preventFlipX)
        {
            if (rb.x != 0)
            { stateMachine.Player.FlipSprite(rb.x < 0); }
        }
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    protected void ForceMove()
    {
        stateMachine.Player.Rigidbody.velocity += stateMachine.Player.ForceReceiver.Movement * Time.deltaTime;
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // 전환되고 있을 때 && 다음 애니메이션이 tag라면
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        // 전환되고 있지 않을 때 && 현재 애니메이션이 tag라면
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
