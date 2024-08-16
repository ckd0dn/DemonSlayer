using UnityEngine;

public class FlyingIdleState : IdleState
{
    public FlyingIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }


    public override void Update()
    {
        if (stateMachine.Monster.isAttacking == false)
        {
            int randomStop = Random.Range(0, 5000);

            if (!isStopping)
            {
                ChangeDirection();
                Flyingmove();
            }

            if (randomStop > 4998 && !isStopping)
            {
                isStopping = true;
            }

            if (isStopping)
            {
                stopStartTime += Time.deltaTime;

                if (stopStartTime >= stopDuration)
                {
                    isStopping = false;
                    stopStartTime = 0f;
                }
            }

            float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);
            var flyingMonster = stateMachine.Monster as FlyingMonster;
            if (!flyingMonster.ChasingDelay)
            {
                if (distance < stateMachine.Monster.canstats.detectRange)
                {
                    stateMachine.ChangeState(stateMachine.chasingState);
                }
            }
        }
    }

    public void Flyingmove()
    {
        float FlyingmoveCycle = 3.0f;
        float Flyingmovesize = 0.5f;

        Vector2 baseDirection = Vector2.right;

        float FlyingOffsetY = Mathf.Sin(Time.time * FlyingmoveCycle) * Flyingmovesize;
        Vector2 FlyingOffset = new Vector2(0, FlyingOffsetY);

        Vector2 movement = (baseDirection + FlyingOffset).normalized * stateMachine.Monster.stats.speed * Time.deltaTime;

        stateMachine.Monster.transform.Translate(movement);
    }
}