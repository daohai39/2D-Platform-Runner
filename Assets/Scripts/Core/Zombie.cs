using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
	[SerializeField] private Collider2D attackRange;

    public override bool IsDead 
	{
		get {
			return health <= 0;
		}
	}

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

    public override IEnumerator TakeDamage()
    {
		health -= 10;
		if (IsDead) {
			Animator.SetTrigger("die");
			yield return null;
		} else {
			Animator.SetTrigger("damage");
		}
    }
}
