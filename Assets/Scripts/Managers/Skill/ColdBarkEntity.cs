using UnityEngine;
using System.Collections.Generic;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class ColdBarkEntity : ISkillEntity
	{

		RaycastHit aimIconHitPoint;

		public ColdBarkEntity (SkillInfo skillInfo) : base (skillInfo)
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
			//播放玩家释放技能的粒子效果
			SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);

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
				this.aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition, "ZomBear|ZomBunny|Hellephant");

				if (aimIconHitPoint.collider == null || (!"ZomBear".Equals (aimIconHitPoint.collider.tag) && !"ZomBunny".Equals (aimIconHitPoint.collider.tag)
					&& !"Hellephant".Equals (aimIconHitPoint.collider.tag))) {
					return;
				}

				//若大于施法距离则给玩家添加自动导航组件
				if (Vector3.Distance (PlayerManager.playerGO.transform.position, this.aimIconHitPoint.collider.transform.position) > this.skillInfo.distance) {
					PlayerManager.ResetMouseCursor ();

					NavigateToTarget (PlayerManager.playerGO.transform, this.aimIconHitPoint.collider.transform.position, this.skillInfo.distance,
						PlayerManager.playerGO.GetComponent<PlayerMovement> ().speed, this.StartSkillLogic);

					return;
				}

				StartSkillLogic ();

			}
			//鼠标右键或方向键可取消释放技能
			else if (Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Cancel")) {
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}

		}

		public void StartSkillLogic ()
		{
			PlayerManager.ResetMouseCursor ();

			base.Sing ();

			this.skillInfo.releasingParticle.transform.position = PlayerManager.playerGO.transform.position;

			float angle = Vector3.Angle (aimIconHitPoint.point, PlayerManager.playerGO.transform.position);

			//Debug.Log (angle);

			PlayerManager.playerGO.transform.rotation = Quaternion.Euler (0f, angle, 0f);

			//播放玩家释放技能时的动画
			PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

			//this.skillInfo.releasingParticle.transform.position = Vector3.MoveTowards (this.skillInfo.releasingParticle.transform.position, aimIconHitPoint, 12.0f * Time.deltaTime);

			this.skillInfo.releasingParticle.GetComponent<DemoParticleControl> ().targetPosition = aimIconHitPoint.point;

			SphereCollider sphereCollider = this.skillInfo.releasingParticle.gameObject.AddComponent<SphereCollider> ();
			sphereCollider.radius = 0.15f;

			ColdSkillDamage skillDamage = this.skillInfo.releasingParticle.gameObject.AddComponent<ColdSkillDamage> ();
			skillDamage.skillEntity = this;

			Release ();
		}
	}
}
