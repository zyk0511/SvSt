using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class ForwardThunderEntity : ISkillEntity
	{
		RaycastHit aimIconHitPoint;

		public ForwardThunderEntity (SkillInfo skillInfo) : base (skillInfo)
		{
		}

		public override void Sing ()
		{
			this.skillInfo.isIconMarking = true;

			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
			SkillManager.skillEntity = this;

		}

		public override void Release ()
		{
			base.Release ();
		}

		public override void HitTarget ()
		{
			base.HitTarget ();
		}

		public override void Complete ()
		{
			base.Complete ();
		}

		public override void Update ()
		{
			if (IsSkillStopped ()) {
				Complete();
				return;
			}

			if (Input.GetMouseButtonDown (0)) {
				//获取AOE图标投射在世界空间的射线
				this.aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition,
					"ZomBear|ZomBunny|Hellephant");

				//投射对象必须是enemy类型
				if (this.aimIconHitPoint.collider == null || (!"ZomBear".Equals (this.aimIconHitPoint.collider.tag) && 
					!"ZomBunny".Equals (this.aimIconHitPoint.collider.tag) && !"Hellephant".Equals (this.aimIconHitPoint.collider.tag))) {
					return;
				}

				//若大于施法距离则给玩家添加自动导航组件
				if (Vector3.Distance (PlayerManager.playerGO.transform.position,
					this.aimIconHitPoint.collider.transform.position) > this.skillInfo.distance) {

					PlayerManager.ResetMouseCursor ();

					NavigateToTarget (PlayerManager.playerGO.transform, this.aimIconHitPoint.collider.transform.position, this.skillInfo.distance,
						PlayerManager.playerGO.GetComponent<PlayerMovement> ().speed, this.StartSkillLogic);

					return;
				}

				StartSkillLogic ();

			} else if (Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Cancel")) {
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}

		public void StartSkillLogic ()
		{
			PlayerManager.ResetMouseCursor ();

			base.Sing ();

			float angle = Vector3.Angle (this.aimIconHitPoint.point, PlayerManager.playerGO.transform.position);

			//Debug.Log (angle);

			PlayerManager.playerGO.transform.rotation = Quaternion.Euler (0f, angle, 0f);

			//播放玩家释放技能时的动画
			PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

			List<GameObject> enemiesAttacked = SkillManager.GetInstance ().GetEnemyListInRangeByAmount (this.aimIconHitPoint.point, 3,
				this.skillInfo.range,"ZomBear|ZomBunny|Hellephant");

			//enemiesAttacked.Insert (0, aimIconHitPoint.collider.gameObject);

			GameObject[] enemiesAttackedArray = enemiesAttacked.ToArray ();

			for (int i = 0; i < enemiesAttackedArray.Length; i++) {
				GameObject sourceOfAttackGO;
				GameObject endOfAttackGO = (GameObject)enemiesAttackedArray.GetValue (i);
				GameObject chainLightning;

				if (i == 0) {
					sourceOfAttackGO = PlayerManager.playerGO;				
				} else {
					sourceOfAttackGO = (GameObject)enemiesAttackedArray.GetValue (i - 1);
				}

				chainLightning = SkillManager.GetInstance ().InstantiateChainLightning (sourceOfAttackGO.transform, endOfAttackGO.transform);
				chainLightning.GetComponent<UVChainLightning> ().ShowForSecondsBeforeDestroying (0.25f);

				PlaySkillAudio ();

				float damageValue = this.skillInfo.intHP * Mathf.Pow (0.85f, (float)i);						

				endOfAttackGO.GetComponent<EnemyHealth> ().TakeDamage (Mathf.RoundToInt (damageValue));
			}

			Release ();

			HitTarget ();

			Complete ();
		}
	}
}