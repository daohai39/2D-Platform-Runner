using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
	private Enemy enemy;
	private float time;
  private float idleTime;

  public void Enter(Enemy enemy)
  {
    this.enemy = enemy;
    time = Time.time;
    idleTime = Random.Range(1,10);
  }

  public void Execute()
  { 
    Idle();
    if (Time.time - time >= idleTime || enemy.Target != null) {
      enemy.ChangeState(new PatrolState());
    } 
  }

  public void Exit()
  {
  }
	public void Idle() 
	{
    enemy.Animator.SetFloat("speed",0);
	}

  public void OnTriggerEnter2D(Collider2D other)
  {
  }
}
