using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState<T> where T : class {
	void Enter(T enemy);
	void Execute();
	void Exit();

	void OnTriggerEnter2D(Collider2D other);
}
