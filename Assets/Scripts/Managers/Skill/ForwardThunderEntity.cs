using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ForwardThunderEntity : ISkillEntity
    {
		public ForwardThunderEntity(SkillInfo skillInfo):base(skillInfo){}

		public override void Sing()
        {
			base.Sing ();

			SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);
            
			Release ();
        }

		public override void Release()
        {
			base.Release ();
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
        }

		public override void HitTarget()
        {
			base.HitTarget ();
			Complete ();
        }

		public override void Complete()
        {
			base.Complete ();
        }

		public override void Update()
		{
			Vector3 mousePosition = Input.mousePosition;

			if (Input.GetMouseButtonDown (0)) {

				//获取AOE图标投射在世界空间的射线
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition, "ZomBear|ZomBunny|Hellephant");

				//Debug.Log (aimIconHitPoint.y);

				if ("ZomBear".Equals (aimIconHitPoint.collider.tag) || "ZomBunny".Equals (aimIconHitPoint.collider.tag)
				    || "Hellephant".Equals (aimIconHitPoint.collider.tag)) {

					//Debug.Log (raycastHit.collider.name);

					PlayerManager.ResetMouseCursor ();

					TriggerSkill ();

					float angle = Vector3.Angle (aimIconHitPoint.point, PlayerManager.playerGO.transform.position);

					//Debug.Log (angle);

					PlayerManager.playerGO.transform.rotation = Quaternion.Euler (0f, angle, 0f);

					//播放玩家释放技能时的动画
					PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

					List<GameObject> enemiesAttacked = SkillManager.GetInstance ().GetEnemyListInRangeByAmount (aimIconHitPoint.point, 3, this.skillInfo.range);

					//enemiesAttacked.Insert (0, aimIconHitPoint.collider.gameObject);

					GameObject[] enemiesAttackedArray = enemiesAttacked.ToArray ();

					for (int i = 0; i < enemiesAttackedArray.Length;i++) {
						GameObject sourceOfAttackGO;
						GameObject endOfAttackGO = (GameObject)enemiesAttackedArray.GetValue (i);
						GameObject chainLightning;

						if (i == 0) {
							sourceOfAttackGO = PlayerManager.playerGO;				
						} else {
							sourceOfAttackGO = (GameObject)enemiesAttackedArray.GetValue (i - 1);					}

						chainLightning = SkillManager.GetInstance ().InstantiateChainLightning (sourceOfAttackGO.transform,endOfAttackGO.transform);
						chainLightning.GetComponent<UVChainLightning> ().ShowForSecondsBeforeDestroying (1f);

						float damageValue = this.skillInfo.intHP * Mathf.Pow (0.85f, (float)i);						

						endOfAttackGO.GetComponent<EnemyHealth> ().TakeDamage (Mathf.RoundToInt(damageValue));

					}

					HitTarget ();
				}

			} else if (Input.GetMouseButtonDown (1)  || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) {
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}

    }
}