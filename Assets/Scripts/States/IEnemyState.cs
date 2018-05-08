using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState {
	void Enter(Enemy enemy);
	void Execute();
	void Exit();

	void OnTriggerEnter2D(Collider2D other);
}
