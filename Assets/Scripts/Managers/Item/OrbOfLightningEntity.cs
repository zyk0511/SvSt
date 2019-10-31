using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class OrbOfLightningEntity : MonoBehaviour
	{
		public ItemInfo itemInfo;

		public void SetItemInfo(ItemInfo itemInfo)
		{
			this.itemInfo = itemInfo;
		}

		// Use this for initialization
		void Start ()
		{
			PlayerManager.playerShooting.damagePerShot += 5;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (PlayerManager.playerShooting.GetIsShooting () && PlayerManager.playerShooting.GetGunLine ().enabled) {

				RaycastHit shootHit = PlayerManager.playerShooting.GetShootHit ();
				if(shootHit.collider != null)
				{
					EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
					EnemyMovement enemyMovement = shootHit.collider.GetComponent <EnemyMovement> ();

					if (enemyHealth != null && enemyMovement != null && !enemyMovement.GetIsSlowedDown () && !"Hellephant".Equals(enemyHealth.gameObject.tag)) {
						ParticleSystem particleSystem = Resources.Load ("_SFB_Particle Package 1/Magic Spells/Magic Sparks", 
							typeof(ParticleSystem)) as ParticleSystem;
						
						//实例化预设体
						ParticleSystem slowingDownParticle = Instantiate (particleSystem);
						enemyMovement.SlowDownEnemyForSeconds (3f, enemyMovement.movingSpeed * 0.8f, slowingDownParticle);
					}
				}
			}
		}
	}
}