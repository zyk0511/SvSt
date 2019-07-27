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
			//Debug.Log(PlayerGameObject.playerStatus.IsDamaged());
			//若该技能可被打断且当前玩家正遭受攻击，则当前技能从吟唱状态回退到准备状态，不再执行下一步动作
			if (this.skillInfo.intCanBeStoppedOrNot == 1 && PlayerManager.playerStatus.GetIsDamaged ()) {
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
				return;
			}

			//SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillSingingState ());
		}

		public override void Release ()
		{
			ReleaseSkill ();
			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillReleasingState ());
		}

		public override void HitTarget ()
		{
			PlayerManager.playerStatus.IncreaseHealth (this.skillInfo.intHP);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillHittingState ());

			Complete ();
		}

		public override void Complete ()
		{
			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
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
					//播放玩家释放技能时的动画
					PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

					//播放玩家释放技能的粒子效果
					//治疗术只能应用于玩家（或队友）自身，所以其粒子效果设置为singingParticle，在初始化时即挂载在PlayerGO节点上
					SkillManager.GetInstance ().PlayParticle (this.skillInfo.singingParticle);

					//技能释放完毕后可进行射击操作
					PlayerManager.playerShooting.SetIsShooting(true);

					HitTarget ();

					SkillManager.skillEntity = null;
				}

			}else if(Input.GetMouseButtonDown (1)){

				//取消释放技能的操作
				PlayerManager.ResetMouseCursor ();
				//恢复之前的魔法值
				PlayerManager.playerMana.IncreaseMana(this.skillInfo.intMP);
				//技能状态机回退到技能准备状态
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState());

				SkillManager.skillEntity = null;

			}
		}

		void ReleaseSkill ()
		{
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
			//禁止射击及释放任何其他技能
			PlayerManager.playerShooting.SetIsShooting(false);
			SkillManager.skillEntity = this;
		}

	}
}