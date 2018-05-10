using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState<Enemy> {
	private Enemy enemy;

	private float time;

	private float patrolTime;
	public void Enter(Enemy enemy)
	{
		this.enemy = enemy;
		time = Time.time;
		patrolTime = Random.Range(1,10);
	}

	public void Execute()
	{
		if (Time.time - time >= patrolTime) {
			enemy.ChangeState(new IdleState());
		} 
		if (enemy.Target != null) {
			if (enemy.IsInMeleeRange) 
				enemy.ChangeState(new MeleeState());
			else if (!enemy.IsInMeleeRange && enemy.IsInThrowRange)
				enemy.ChangeState(new RangedState());
		}
		Patroling();
		enemy.Move();
	}

	public void Exit()
	{
	}

	public void Patroling() 
	{
		enemy.Animator.SetFloat("speed",1);
	}
	public void OnTriggerEnter2D(Collider2D other)
	{
	}
}
