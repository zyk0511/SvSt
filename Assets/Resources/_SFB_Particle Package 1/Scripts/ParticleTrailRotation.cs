using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrailRotation : MonoBehaviour {

	// Update is called once per frame
	Vector3 rotationSpeed;
	bool local = true;

	void Update(){
		if (local)
			transform.Rotate(rotationSpeed * Time.deltaTime);
		else
			transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
	}
}
