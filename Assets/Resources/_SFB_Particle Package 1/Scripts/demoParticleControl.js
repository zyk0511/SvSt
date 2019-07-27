﻿#pragma strict


var startPosition		: Vector3		= Vector3(0,0.1,0);		// Where should this particle start?
var startRotation		: Vector3		= Vector3(0,0,0);		// Rotation should this particle start?
var shootsTarget		: boolean		= false;				// If true, particle will shoot forward
var shootSpeed			: float			= 12.0;					// Multiplier of Time.deltaTime
var targetPosition		: Vector3		= Vector3(0,0.1,100);



function Start () {
	transform.position = startPosition;
}

function Update () {
	if (shootsTarget)
		transform.position = Vector3.MoveTowards(transform.position,targetPosition,shootSpeed * Time.deltaTime);
		//transform.position.z += shootSpeed * Time.deltaTime;
}