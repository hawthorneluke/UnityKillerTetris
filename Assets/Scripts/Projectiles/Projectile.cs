using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float damage; //time goes down by this amount if player is hit

	public float GetDamage() {
		return damage;
	}
}
