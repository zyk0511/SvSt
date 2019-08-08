using UnityEngine;
using System.Collections;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class FirstAidForSingleEntity : ISkillEntity
	{
		RaycastHit aimIconRaycastHit;

		public FirstAidForSingleEntity (SkillInfo skillInfo) : base (skillInfo)
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
			PlayerManager.playerStatus.IncreaseHealth (this.skillInfo.intHP);

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
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition,"Player");

				if ("Player".Equals(aimIconHitPoint.collider.tag)) {

					//Debug.Log (raycastHit.collider.name);

					PlayerManager.ResetMouseCursor ();

					TriggerSkill ();

					//播放玩家释放技能时的动画
					PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

					//播放玩家释放技能的粒子效果
					//治疗术只能应用于玩家（或队友）自身，所以其粒子效果设置为singingParticle，在初始化时即挂载在PlayerGO节点上
					SkillManager.GetInstance ().PlayParticle (this.skillInfo.singingParticle);

					HitTarget ();
				}

			}else if(Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")){
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}
	}
}