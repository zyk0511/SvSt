using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrailRotation : MonoBehaviour {

	// Update is called once per frame
	public Vector3 rotationSpeed;
	public bool local = true;
	public bool isParticlePlaying = false;

	void Update(){

		if(isParticlePlaying)
		{
			if (local)
				transform.Rotate(rotationSpeed * Time.deltaTime);
			else
				transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
		}
	}
}
