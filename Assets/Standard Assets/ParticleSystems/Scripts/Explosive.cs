using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

public class Explosive : MonoBehaviour
{
    public Transform explosionPrefab;
	public float sizeMultiplier = 1;

    private bool m_Exploded;
	
    public void Explode()
    {
		if (!m_Exploded)
		{
			Instantiate(explosionPrefab, this.transform.position,
			            Quaternion.LookRotation(this.transform.position.normalized));
			m_Exploded = true;
		}
    }
}
