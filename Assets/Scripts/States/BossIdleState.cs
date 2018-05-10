using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLandState : IEnemyState<Boss>
{
    private Boss boss;
    public void Enter(Boss boss)
    {
        Debug.Log("landing");
        this.boss = boss;
    }

    public void Execute()
    {
        Land();
        if(boss.IsGrounded()) {
            Debug.Log("switch to idle");
            boss.ChangeState(new BossIdleState());
        }
    }

    public void Exit()
    {
        boss.Animator.SetBool("land", false);
        boss.Animator.SetLayerWeight(1,0);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Land()
    {
        boss.Animator.SetBool("land", true);
    }
}

public class BossJumpState : IEnemyState<Boss>
{
    private Boss boss;
    private bool canJump;//for test
    public void Enter(Boss boss)
    {
        this.boss = boss;
        canJump = true;//for test
    }

    public void Execute()
    {
        if (canJump)
        {
            canJump = false;
            Jump();
        }
        if (boss.IsLanding) {
            boss.ChangeState(new BossLandState());
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Jump");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Jump()
    {
        boss.Animator.SetTrigger("jump");
        boss.Animator.SetLayerWeight(1,1);
        boss.Jump();
    }
}

public class BossRunState : IEnemyState<Boss>
{
    private float runDuration;
    private float runTimer;
    private Boss boss;
    public void Enter(Boss boss)
    {
        this.boss = boss;
        runDuration = 5;
        runTimer = Time.time;
    }

    public void Execute()
    { 
        Run();
        if (Time.time - runTimer >= runDuration) {
            boss.ChangeState(new BossJumpState());
        }
    }

    public void Exit()
    {
        boss.Animator.SetFloat("speed",0);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Run() 
    {
        boss.Animator.SetFloat("speed",1);
        boss.Move();
    }
}

public class BossWalkState : IEnemyState<Boss>
{
    private Boss boss;

    private float walkDuration;
    private float walkTimer;

    public void Enter(Boss boss)
    {
        this.boss = boss;
        walkDuration = 5;
        walkTimer = Time.time;
    }

    public void Execute()
    {
        Walk();
        if (Time.time - walkTimer >= walkDuration) {
            boss.ChangeState(new BossJumpState());
        }
    }

    public void Exit()
    {
        boss.Animator.ResetTrigger("walk");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Walk()
    {
        boss.Animator.SetTrigger("walk");
        boss.Move();
    }
}

public class BossIdleState : IEnemyState<Boss>
{
    private float idleDuration;
    private float idleTimer;
    private Boss boss;
    public void Enter(Boss boss)
    {
        this.boss = boss;
        idleDuration = 5;
        idleTimer = Time.time;
    }

    public void Execute()
    {
        Idle();
        if(Time.time - idleTimer >= idleDuration) {
            if (!boss.IsInRage)
                boss.ChangeState(new BossWalkState());
            else 
                boss.ChangeState(new BossRunState());
        }
    }

    public void Exit()
    {
        boss.IsVulnerable = false;
        boss.EnableAttack();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Idle()
    {
        boss.Animator.SetFloat("speed",0);
        boss.IsVulnerable = true;
        boss.DeactivateAttack();
        boss.Rgbody.velocity = Vector2.zero;
    }
}
