using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : Enemy
{   
	[SerializeField] private Collider2D swordCollider;

    [SerializeField] private Transform knifePos;
    [SerializeField] private GameObject knifePrefab;

    public override bool IsDead {
        get { 
            return health <= 0;
        }
    }
    public override void Move()
    {
        if (!Attack) {
            Animator.SetFloat("speed", 1);
            transform.Translate(speed * GetDirection() * Time.deltaTime);
        }
    }

	public override void PerformAttack()
	{
		swordCollider.enabled = !swordCollider.enabled;
	}

    public override void Die()
    {
        gameObject.SetActive(false);
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

    

    public void ThrowKnife() 
    {
        if (isFacingRight) {
            GameObject tmp = (GameObject) Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0,0,-90));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        } else {
            GameObject tmp = (GameObject) Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0,0,90));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

}
