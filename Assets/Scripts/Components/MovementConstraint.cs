using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementConstraint : MonoBehaviour {
	[SerializeField] private Boundary boundary;
	[SerializeField] private GameObject player;
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		player.transform.position = new Vector2(
			Mathf.Clamp(player.transform.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(player.transform.position.y, boundary.yMin, boundary.yMax)
		);
	}

}
