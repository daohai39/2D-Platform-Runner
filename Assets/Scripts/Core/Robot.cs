using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy {
	[SerializeField] private Collider2D swordCollider;
	[SerializeField] private Transform shotPos;
	[SerializeField] private GameObject shotPrefab;

    public override bool IsDead {
		get {
			return health <= 0;
		}
	}

    public override void Die() {
		DestroySelf();
    }

    public override void Move() {
		if (!IsOnEdge()){
			if (!Attack) {
				Animator.SetFloat("speed", 1);
				transform.Translate(speed * GetDirection() * Time.deltaTime);
			}
		} else if (currentState is PatrolState) {
			ChangeDirection();
		}
    }

    public override void PerformAttack() {
		swordCollider.enabled = !swordCollider.enabled;
    }

    public override IEnumerator TakeDamage() {
		health -= 10;
		if (IsDead) {
			Animator.SetTrigger("die");
			yield return null;
		} else {
			Animator.SetTrigger("damage");
		}
    }

	public void Shoot() {
		if (isFacingRight) {
			GameObject tmp = (GameObject)Instantiate(shotPrefab, shotPos.position,Quaternion.Euler(0,0,-180));
			tmp.GetComponent<RangeWeapon>().Initialize(Vector2.right);
		} else {
			GameObject tmp = (GameObject)Instantiate(shotPrefab, shotPos.position,Quaternion.Euler(0,0,180));
			tmp.GetComponent<RangeWeapon>().Initialize(Vector2.left);
		}
	}

}
