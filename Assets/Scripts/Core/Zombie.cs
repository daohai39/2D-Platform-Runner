using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
	[SerializeField] private Collider2D attackRange;


    public override void Die()
    {
		DestroySelf();
    }

    public override void Move()
    {
		if (!IsOnEdge()) {
			Animator.SetFloat("speed", 1);
			if (!Attack) {
				if (Target)
					transform.Translate(speed * GetDirection() * Time.deltaTime * 2);
				else transform.Translate(speed * GetDirection() * Time.deltaTime);
			}
		} else if (currentState is PatrolState) {
				ChangeDirection();
		}
    }

    public override void PerformAttack()
    {
			attackRange.enabled = !attackRange.enabled;
    }

}
