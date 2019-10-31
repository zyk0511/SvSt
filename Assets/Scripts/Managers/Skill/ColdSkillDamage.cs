using UnityEngine;
//using System.Collections.Generic;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class ColdSkillDamage:MonoBehaviour
	{
		public ISkillEntity skillEntity;

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
					skillEntity.PlaySkillAudio ();

					enemyGO.GetComponent<EnemyHealth> ().TakeDamage (skillEntity.skillInfo.intHP);

					enemyGO.GetComponent<EnemyMovement> ().FreezeEnemyForSeconds (2f);

					hitTargetSum++;

					SkillManager.GetInstance ().StopParticle (skillEntity.skillInfo.releasingParticle);

					skillEntity.HitTarget ();
					skillEntity.Complete ();
				}
			}

		}
	}
}