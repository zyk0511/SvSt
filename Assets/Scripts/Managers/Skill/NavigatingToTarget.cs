using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class NavigatingToTarget : MonoBehaviour
	{
		public Transform startTrans;
		public Vector3 targetPosition;
		public float distance;
		public float speed;
		public Action action; 

		NavMeshAgent nav;
		Animator anim;

		// Use this for initialization
		void Start ()
		{
			nav = startTrans.gameObject.AddComponent<NavMeshAgent> ();
			nav.speed = this.speed;
			nav.enabled = true;

			anim = startTrans.gameObject.GetComponent<Animator> ();

		}
	
		// Update is called once per frame
		void Update ()
		{
			anim.SetBool ("Run", true);
			nav.SetDestination (targetPosition);

			if (Input.GetMouseButton (1) || Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) {
				Destroy (this);
				return;
			}

			if (Vector3.Distance (startTrans.position, targetPosition) <= distance) {
				action ();
				Destroy (this);
				return;
			}
		}

		void OnDestroy()
		{
			anim.SetBool ("Run", false);
			nav.speed = 0f;
			nav.enabled = false;
			Destroy (nav);
		}
	}
}