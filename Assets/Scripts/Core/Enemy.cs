using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour {
	private int id;

	public int Id { get; set; }
	
	public float AttackCd { get { return attackCd; } }

	public Animator Animator { get; private set; }

	public Rigidbody2D Rigidbody2D { get; private set; }

	public GameObject Target { get; set; }

	public bool Attack { get; set; }

	private bool isDead;

	public virtual bool IsDead { 
		get {
			return health.CurrentHealth <= 0;
		}
		set {
			if (value == true) {
				health.CurrentHealth = 0;
			}
			isDead = value;
		}
	}

	protected bool isFacingRight;

	protected IEnemyState currentState;

	[SerializeField] private int point;

  	[SerializeField] private float attackCd = 5;

	[SerializeField] protected float speed;
    [SerializeField] private float throwRange;
    [SerializeField] private float meleeRange;

	[SerializeField] protected List<string> damageSources;

	[SerializeField] private Transform leftEgde;
	[SerializeField] private Transform rightEgde;
	[SerializeField] private Health health;
    public bool IsInMeleeRange {
        get {
            if(Target == null) return false;
            return Vector2.Distance(Target.transform.position, transform.position) <= meleeRange;
        }
    }
    public bool IsInThrowRange {
        get {
            if(Target == null) return false;
            return Vector2.Distance(Target.transform.position, transform.position) <= throwRange;
        }
    }


	// Use this for initialization
	protected virtual void Start () {
		health = GetComponent<Health>();
		IsDead = false;
		Animator = GetComponent<Animator>();
		Rigidbody2D = GetComponent<Rigidbody2D>();	
		isFacingRight = true;
		ChangeState(new IdleState());
	}
	

	protected virtual void Update () 
	{
		if(!IsDead){
			currentState.Execute();
			LookAtTarget();
		}
	}


	protected Vector2 GetDirection()
	{
		return isFacingRight ? Vector2.right : Vector2.left;
	}


	protected virtual void LookAtTarget() 
	{
		if (Target != null) {
			var xDir = Target.transform.position.x - transform.position.x;
			if (xDir > 0 && !isFacingRight || xDir < 0 && isFacingRight) {
				ChangeDirection(); 
			}
		}
	}

	protected bool IsOnEdge()
	{
		return (GetDirection().x > 0 && transform.position.x >= rightEgde.position.x 
		|| GetDirection().x < 0 && transform.position.x <= leftEgde.position.x);
	}

	public void ChangeState(IEnemyState newState) 
	{
		if (newState == null)
			return;
		if (currentState != null)
			currentState.Exit();
		currentState = newState;
		currentState.Enter(this);
	}

	public void ChangeDirection()
	{
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector2(transform.localScale.x * - 1, transform.localScale.y);
	}

	public virtual void DestroySelf() 
	{
		gameObject.SetActive(false);
	}

	public virtual IEnumerator TakeDamage(int amount) 
	{
		health.TakeDamage(amount);
        if (IsDead) {
            Animator.SetTrigger("die");
            yield return null;
        } else {
            Animator.SetTrigger("damage");
        }
	}

	private	void OnTriggerEnter2D(Collider2D other)
	{
		if (IsDead) return;
		if (damageSources.Contains(other.tag)) {		
			Debug.Log((other.GetComponent<DamageManagement>().DamageOutput));	
			StartCoroutine(TakeDamage(other.GetComponent<DamageManagement>().DamageOutput));
			if(other.tag == "Knife") Destroy(other.gameObject); // Destroy knife prefab or else it will collide with enemy sight
		}
		
	}

	// public abstract void Idle();
	public abstract void Move();	
	public abstract void PerformAttack();

	public abstract void Die();
	

}
