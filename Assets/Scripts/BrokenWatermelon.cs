using UnityEngine;
using System.Collections;

public class BrokenWatermelon : MonoBehaviour {

	public float destroyTime = 1.0f;

	void Start () {
		//play sound
		Destroy (this.gameObject, destroyTime);
	}
}
