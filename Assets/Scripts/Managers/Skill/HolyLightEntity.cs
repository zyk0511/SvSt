using UnityEngine;
using System.Collections;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class HolyLightEntity : ISkillEntity
    {
		public HolyLightEntity(SkillInfo skillInfo):base(skillInfo){}

		public override void Sing()
        {
			this.skillInfo.isIconMarking = true;

			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
			SkillManager.skillEntity = this;
        }

		public override void Release()
        {
			SkillManager.GetInstance ().PlayParticle (this.skillInfo.singingParticle);

			DivineShield divineShield = PlayerManager.playerGO.AddComponent<DivineShield> ();
			divineShield.skillEntity = this;

			base.Release ();
		}

		public override void HitTarget()
        {
			base.HitTarget ();
        }

		public override void Complete()
        {
			base.Complete ();
        }

		public override void Update()
		{
			if (IsSkillStopped ()) {
				Complete();
				return;
			}

			if (Input.GetMouseButtonDown (0)) {
				//获取AOE图标投射在世界空间的射线
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition,"Player");

				if (aimIconHitPoint.collider != null && "Player".Equals(aimIconHitPoint.collider.tag)) {

					PlayerManager.ResetMouseCursor ();

					base.Sing ();

					Release ();

					HitTarget ();
				}
			}else if(Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Cancel")){
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}
    }
}
