using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoParticleControl : MonoBehaviour {

	public Vector3 startPosition = new Vector3(0f,0.1f,0f);		// Where should this particle start?
	public Vector3 startRotation = new Vector3(0f,0f,0f);		// Rotation should this particle start?
	public bool shootsTarget = false;				// If true, particle will shoot forward
	public float shootSpeed = 12.0f;					// Multiplier of Time.deltaTime
	public Vector3 targetPosition = new Vector3(0f,0.1f,100f);



	void Start () {
		transform.position = startPosition;
	}

	void Update () {
		if (shootsTarget) 
		{
			transform.position = Vector3.MoveTowards(transform.position,targetPosition,shootSpeed * Time.deltaTime);
		}
			
		//transform.position.z += shootSpeed * Time.deltaTime;
	}
}
