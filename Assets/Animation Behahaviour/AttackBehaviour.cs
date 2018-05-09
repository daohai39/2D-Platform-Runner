using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(animator.GetComponent<Enemy>() != null) {
			animator.GetComponent<Enemy>().Attack = true;
			animator.SetFloat("speed", 0);
		} else if (animator.GetComponent<Player>() != null) {
			Player.Instance.Attack = true;
			if (Player.Instance.OnGround)
				Player.Instance.Rigidbody.velocity = Vector2.zero;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(animator.GetComponent<Enemy>() != null) {
			animator.GetComponent<Enemy>().Attack = false;
			animator.GetComponent<Enemy>().PerformAttack();
		} else if (animator.GetComponent<Player>() != null) {
			Player.Instance.Attack = false;
			Player.Instance.MeleeAttack();
			Player.Instance.Animator.ResetTrigger("attack");
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	// override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	// }
}
