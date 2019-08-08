using UnityEngine;
using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ColdBarkEntity : ISkillEntity
	{

		public ColdBarkEntity (SkillInfo skillInfo) : base (skillInfo)
		{
		}

		public override void Sing ()
		{
			base.Sing ();

			SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);

			Release ();

		}

		public override void Release ()
		{
			base.Release ();
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
		}

		public override void HitTarget ()
		{
			base.HitTarget ();
			Complete ();
		}

		public override void Complete ()
		{
			base.Complete ();
		}

		public override void Update ()
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
					skillDamage.skillInfo = this.skillInfo;

					//播放玩家释放技能的粒子效果
					SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);

					HitTarget ();
				}

			}
			//鼠标右键或方向键可取消释放技能
			else if (Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) {
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}
	}
}
