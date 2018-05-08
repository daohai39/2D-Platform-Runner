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
      Debug.Log(canAttack);
      Debug.Log(time);
      Attack();
      enemy.Move();
      if (enemy.Target == null) {
        enemy.ChangeState(new IdleState());
      } 
      if (!enemy.IsInMeleeRange && enemy.IsInThrowRange) {
        enemy.ChangeState(new RangedState());
      }
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
      } else {
        time += Time.deltaTime;
      }
      if (canAttack) {
        canAttack = false;
        time = 0;
        enemy.Animator.SetTrigger("attack");
      } 
  }
  public void OnTriggerEnter2D(Collider2D other)
  {
  }
}
