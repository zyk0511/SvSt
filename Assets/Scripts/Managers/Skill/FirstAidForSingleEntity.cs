using UnityEngine;
using System.Collections;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class FirstAidForSingleEntity : ISkillEntity
	{
		RaycastHit aimIconRaycastHit;

		public FirstAidForSingleEntity (SkillInfo skillInfo) : base (skillInfo)
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
			//播放玩家释放技能时的动画
			PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

			//播放玩家释放技能的粒子效果
			//治疗术只能应用于玩家（或队友）自身，所以其粒子效果设置为singingParticle，在初始化时即挂载在PlayerGO节点上
			SkillManager.GetInstance ().PlayParticle (this.skillInfo.singingParticle);

			PlaySkillAudio ();

			base.Release ();
		}

		public override void HitTarget ()
		{
			PlayerManager.playerStatus.IncreaseHealth (this.skillInfo.intHP);

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
				//获取技能图标投射在世界空间的射线
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition,"Player");

				if (aimIconHitPoint.collider != null && "Player".Equals(aimIconHitPoint.collider.tag)) {
					//Debug.Log (raycastHit.collider.name);
					PlayerManager.ResetMouseCursor ();

					base.Sing ();

					Release ();

					HitTarget ();

					Complete ();
				}

			}else if(Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Cancel")){
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}
	}
}