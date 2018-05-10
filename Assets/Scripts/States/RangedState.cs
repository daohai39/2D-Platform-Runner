using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState<Enemy> {
	private Enemy enemy;
  private float attackCd = 5;
  private float time;
  private bool canAttack = true;

    public void Enter(Enemy enemy)
    {
		this.enemy = enemy;
    }

    public void Execute()
    {
      Attack();
      enemy.Move();
      if (enemy.Target == null) {
        enemy.ChangeState(new IdleState());
      } 
      if (enemy.IsInMeleeRange) {
        enemy.ChangeState(new MeleeState());
      }
    }

  public void Exit()
  {
    enemy.Animator.ResetTrigger("rangeAttack");
  }

  public void Attack()
  {
      if (time >= attackCd) {
        canAttack = true;
      } else {
        time += Time.deltaTime;
      }
      if (canAttack) {
        canAttack = false;
        time = 0;
        enemy.Animator.SetTrigger("rangeAttack");
      } 
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
  }
}
