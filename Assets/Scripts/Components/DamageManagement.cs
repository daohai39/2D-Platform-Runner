using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManagement : MonoBehaviour {
	[SerializeField] private int damageOutput;

	public int DamageOutput { 
		get {
			return damageOutput;	
		}
	 }
}
