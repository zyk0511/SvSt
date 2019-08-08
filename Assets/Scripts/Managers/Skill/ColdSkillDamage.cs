using UnityEngine;
//using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ColdSkillDamage:MonoBehaviour
	{
		public SkillInfo skillInfo;

		int hitTargetSum = 0;

		GameObject enemyGO;

		void OnCollisionEnter(Collision other)
		{
//			foreach (ContactPoint contact in other.contacts)
//			{
//				Debug.DrawRay(contact.point, contact.normal, Color.white);
//			}
			if (hitTargetSum < 1) 
			{
				enemyGO = other.collider.gameObject;
				if ("ZomBear".Equals (enemyGO.tag) || "ZomBunny".Equals (enemyGO.tag) || "Hellephant".Equals (enemyGO.tag)) 
				{
					enemyGO.GetComponent<EnemyHealth> ().TakeDamage (50);
					enemyGO.GetComponent<EnemyMovement> ().FreezeEnemyForSeconds (2f);
					hitTargetSum++;

					SkillManager.GetInstance ().StopParticle (this.skillInfo.releasingParticle);
				}
			}

		}
	}
}

