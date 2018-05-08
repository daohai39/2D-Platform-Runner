using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState {
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
      if (enemy.Target == null) {
        enemy.ChangeState(new IdleState());
      } 
      if (!enemy.IsInMeleeRange && enemy.IsInThrowRange) {
        enemy.ChangeState(new RangedState());
      }
      Attack();
      enemy.Move();
  }

  public void Exit()
  {
	  enemy.Attack = false;
    enemy.Animator.ResetTrigger("attack");
  }

  public void Attack()
  {
      if (time >= attackCd) {
        canAttack = true;
        time = 0;
      } 
      if (canAttack) {
        canAttack = false;
        time += Time.deltaTime;
        enemy.Animator.SetTrigger("attack");
      } 
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
  }
}
