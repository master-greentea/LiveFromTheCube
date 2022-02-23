using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartComponent : MonoBehaviour
{
	public ParticleSystem particles;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A) == true && particles != null)
		{
			particles.Play();
		}
	}

}
