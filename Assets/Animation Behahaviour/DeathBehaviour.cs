using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour {
	private float respawnCoolDown = 5;
	private float deathTimer;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		deathTimer = 0;
		if (animator.GetComponent<Player>() != null)
			animator.GetComponent<Player>().Rigidbody.velocity = Vector2.zero;
		else if (animator.GetComponent<Enemy>() != null)
			animator.GetComponent<Enemy>().Rigidbody2D.velocity = Vector2.zero;
		else if (animator.GetComponent<Boss>() != null)
			animator.GetComponent<Boss>().Rgbody.velocity = Vector2.zero;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		deathTimer += Time.deltaTime;
		if (deathTimer >= respawnCoolDown) {
			if (animator.GetComponent<Player>() != null)
				animator.GetComponent<Player>().Respawn();
			else if (animator.GetComponent<Enemy>() != null)
				animator.GetComponent<Enemy>().Die();
			else if (animator.GetComponent<Boss>() != null)
				animator.GetComponent<Boss>().Die();
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
