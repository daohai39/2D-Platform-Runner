using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
	private int id;
	public int Id { get; set; }
	private bool isDead;
	public bool IsDead {
		get {
			return health.CurrentHealth <= 0;
		}
		set {
			if(value == true) {
				health.CurrentHealth = 0;
			}
			isDead = value;
		}
	}
	
	public bool IsInRage {
		get {
			return health.CurrentHealth < (health.MaxHealth/2);
		}
	}

	public bool IsAtEdge {
		get {
			return (transform.position.x <= boundary.xMin || transform.position.x >= boundary.xMax);
		}
	}

	private bool isVulnerable;

	public bool IsVulnerable {
		get {
			return isVulnerable;
		}
		set {
			isVulnerable = value;
		}
	}
	public Animator Animator { get; set; }

	public Rigidbody2D Rgbody { get; set; }
	[SerializeField] private Boundary boundary;
	[SerializeField] private List<Transform> groundChecks;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private Collider2D attackTriggerRadius;
	[SerializeField] private float groundRadius;
	[SerializeField] private List<string> damageSources;
	[SerializeField] private float jumpForce = 800;
	[SerializeField] private float walkSpeed = 30;
	[SerializeField] private float runSpeed = 50;
	
	[SerializeField] private bool isFacingRight = true;

	private Health health;
	private IEnemyState<Boss> currentState;

	private GameObject target;

	private Vector2 currentPos;
	private Vector2 previousPos;


	public bool IsLanding {
		get {
			return currentPos.y - previousPos.y < 0;
		}
	}

	
	// Use this for initialization
	private void Start () {
		Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
		Animator = GetComponent<Animator>();
		Rgbody = GetComponent<Rigidbody2D>();
		health = GetComponent<Health>();	
		target = Player.Instance.gameObject;
		currentPos = transform.position;
		previousPos = transform.position;
		IsVulnerable = true;
		ChangeState(new BossIdleState());
	}
	
	// Update is called once per frame
	private void Update () {
		if (!IsDead)
		{
			UpdatePosition();
			currentState.Execute();
		}
		if (target == null || !Player.Instance.IsDead)
			target = Player.Instance.gameObject;
		LookAtTarget();
	}

	private void UpdatePosition() {
		previousPos = currentPos;
		currentPos = transform.position;
	}


	protected void LookAtTarget()
	{
		if (target != null) {
			var xDir = target.transform.position.x - transform.position.x;
			if (xDir > 0 && !isFacingRight || xDir < 0 && isFacingRight) {
				ChangeDirection();
			}
		}
	}

	protected Vector2 GetDirection() 
	{
		return isFacingRight ? Vector2.right : Vector2.left;
	}

	protected void ChangeDirection()
	{
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector2(
			transform.localScale.x * -1,
			transform.localScale.y
		);
	}
	public virtual void Die()
	{
		DestroySelf();
	}

	public void ChangeState(IEnemyState<Boss> newState) 
	{
		if (currentState != null) {
			currentState.Exit();
		}
		currentState = newState;
		currentState.Enter(this);
	}

	public void Move()
	{
		if (!IsInRage) {
			transform.Translate(walkSpeed * GetDirection() * Time.deltaTime);
		} else {
			transform.Translate(runSpeed * GetDirection() * Time.deltaTime);
		} 
		if (IsAtEdge) {
			ChangeDirection();
		}
	}
	
	//Almost accurate
	public void Jump() 
	{
		var targetPos = target.transform.position;
		float gravity = Physics.gravity.magnitude;
		float angle = 70 * Mathf.Deg2Rad;
		Vector2 planarTarget = new Vector2(targetPos.x, targetPos.y);
		Vector2 planarPosition = new Vector2(transform.position.x, transform.position.y);
		float distance = Vector2.Distance(planarTarget, planarPosition);
		float initialVelocity = (1/Mathf.Cos(angle)) * Mathf.Sqrt((0.5f  * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle))); 
		Vector2 velocity = new Vector2(initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector2.Angle(Vector2.up, planarTarget - planarPosition);
        Vector2 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector2.right) * velocity;
		if (!isFacingRight) {
			finalVelocity.x = finalVelocity.x * -1;
		}
		Rgbody.velocity = finalVelocity/3;
		Rgbody.AddForce(Vector2.up*jumpForce);
	}
	
	public bool IsGrounded() 
	{
		if (Rgbody.velocity.y <= 0) //whether character is jumping or not
		{
			foreach (Transform groundCheck in groundChecks) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
				foreach (Collider2D collider in colliders) {
					if (collider.gameObject != gameObject) return true;
				}
			}
		}
		return false;
	}
	public void DestroySelf()
	{
		gameObject.SetActive(false);
	}

	public IEnumerator TakeDamage(int amount) 
	{
		if (IsVulnerable) {
			health.TakeDamage(amount);
			if (IsDead) {
                Animator.SetLayerWeight(1,0);
				Animator.SetTrigger("die");
				yield return null;
			} else {
                Animator.SetLayerWeight(1,0);
				Animator.SetTrigger("damage");
			}
		}
	}
	
	public void EnableAttack()
	{
		attackTriggerRadius.enabled = true;
	}

	public void DeactivateAttack()
	{
		attackTriggerRadius.enabled = false;
	}

	public void RemoveTarget()
	{
		target = null;
		ChangeState(new BossIdleState());
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
}
