using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {
	private Enemy enemy;

	private float time;

	private float patrolTime;
	public void Enter(Enemy enemy)
	{
		Debug.Log("Patroling");
		this.enemy = enemy;
		time = Time.time;
		patrolTime = Random.Range(1,10);
	}

	public void Execute()
	{
		Patroling();
		enemy.Move();
		if (Time.time - time >= patrolTime) {
			enemy.ChangeState(new IdleState());
		} else if (enemy.Target != null) {
			if (enemy.IsInMeleeRange) 
				enemy.ChangeState(new MeleeState());
			else if (enemy.IsInThrowRange)
				enemy.ChangeState(new RangedState());
		}
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
