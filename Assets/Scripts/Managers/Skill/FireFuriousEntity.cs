using UnityEngine;
using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class FireFuriousEntity : ISkillEntity
	{
		Vector3 centreOfAOECircle;

		public FireFuriousEntity (SkillInfo skillInfo) : base (skillInfo){}

		public override void Sing ()
		{
			//Debug.Log(PlayerManager.playerStatus.GetAnimtor ().GetCurrentAnimatorStateInfo (PlayerManager.playerStatus.GetAnimtor ().GetLayerIndex("Base Layer")).IsName ("Base Layer.Idle"));
			//玩家释放技能前先回到idle状态
			PlayerManager.playerStatus.GetAnimtor ().SetBool ("Run", false);

			//Debug.Log(PlayerManager.playerStatus.GetIsDamaged());
			//若该技能可被打断且当前玩家正遭受敌方的法术中断，则当前技能从吟唱状态回退到准备状态，不再执行下一步动作
			if (this.skillInfo.intCanBeStoppedOrNot == 1 && PlayerManager.playerStatus.GetIsSuspended ()) {
				//PlayerManager.playerStatus.GetAnimtor().SetTrigger("Idle");
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
				return;
			}

			//Debug.Log(PlayerManager.GetCurrentAnimatorStateInfo ().IsName ("Base Layer.Idle"));

			//PlayerManager.playerStatus.GetAnimtor().SetBool("Run",false);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillSingingState ());

			//Release ();

		}

		public override void Release ()
		{
			//Debug.Log (this.skillStateMachine.GetSkillState());
			ReleaseSkill ();
		}

		public override void HitTarget ()
		{
			//计算技能图标圆心与右切点在世界空间的实际作用距离即作用半径
			float radiusSquare = SkillManager.GetInstance ().GetRadiusSquareOfSkillCircleInWorldSpace (100f, Input.mousePosition, this.centreOfAOECircle);

			//Debug.Log (radiusSquare);

			SkillManager.GetInstance ().DamageAllEnemiesInCircle (this.centreOfAOECircle, radiusSquare, this.skillInfo.intHP);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillHittingState ());

			Complete ();
		}

		public override void Complete ()
		{
			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
		}

		public override void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {

				PlayerManager.ResetMouseCursor ();
				//播放玩家释放技能时的动画
				PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

				//获取AOE图标中心的世界坐标
				this.centreOfAOECircle = SkillManager.GetInstance ().GetCentreOfSkillCircleInWorldSpace (100f, Input.mousePosition);

				//将releasingParticle对象的世界坐标设置为AOE图标中心的世界坐标
				this.skillInfo.releasingParticle.transform.position = this.centreOfAOECircle;

				//播放玩家释放技能的粒子效果
				SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);
				//技能状态机进入到技能释放状态
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillReleasingState ());

				//技能释放完毕后可进行射击操作
				PlayerManager.playerShooting.SetIsShooting(true);

				HitTarget ();

				SkillManager.skillEntity = null;


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
			PlayerManager.SetMouseCursor ("AOEIcon1");

			//禁止射击及释放任何其他技能
			PlayerManager.playerShooting.SetIsShooting(false);

			SkillManager.skillEntity = this;
		}

	}
}
