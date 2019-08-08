using UnityEngine;
using System.Collections;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ISkillEntity
    {
		public SkillInfo skillInfo;

		public SkillStateMachine skillStateMachine;

		//public SkillManager skillManager;

		public ISkillEntity(SkillInfo skillInfo)
		{
			this.skillInfo = skillInfo;
			//this.skillManager = SkillManager.GetInstance();
		}

		public SkillStateMachine GetSkillStateMachine()
		{
			return this.skillStateMachine;
		}

		public void SetSkillStateMachine(SkillStateMachine skillStateMachine)
		{
			this.skillStateMachine = skillStateMachine;
		}

		public virtual void Sing (){
			//Debug.Log(PlayerManager.playerStatus.GetAnimtor ().GetCurrentAnimatorStateInfo (PlayerManager.playerStatus.GetAnimtor ().GetLayerIndex("Base Layer")).IsName ("Base Layer.Idle"));
			//玩家释放技能前先回到idle状态
			PlayerManager.playerStatus.GetAnimtor ().SetBool ("Run", false);

			//Debug.Log(PlayerManager.playerStatus.GetIsDamaged());
			//若该技能可被打断且当前玩家正遭受敌方的法术中断，则当前技能从吟唱状态回退到准备状态，不再执行下一步动作
			if (this.skillInfo.intCanBeStoppedOrNot == 1 && PlayerManager.playerStatus.GetIsDamaged()) {
				//PlayerManager.playerStatus.GetAnimtor().SetTrigger("Idle");
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
				return;
			}

			this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillSingingState());

			//Debug.Log(PlayerManager.GetCurrentAnimatorStateInfo ().IsName ("Base Layer.Idle"));
		}

		public virtual void Release (){
			//禁止射击
			PlayerManager.playerShooting.SetIsShooting (false);
			SkillManager.skillEntity = this;

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillReleasingState ());
		}

		public virtual void HitTarget (){
			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillHittingState ());
		}

		public virtual void Complete (){

			UnMarkSkillImg ();

			//技能释放完毕后可进行射击操作
			PlayerManager.playerShooting.SetIsShooting (true);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());

			SkillManager.skillEntity = null;
		}

		public virtual void Update(){
			//PlayerCastingSkill playerCastingSkill = (PlayerCastingSkill)this.skillStateMachine.GetSender ();
			//playerCastingSkill.SetImgFillAmount ();
		}

		//触发技能启动并扣除魔法值
		public void TriggerSkill()
		{
			PlayerCastingSkill playerCastingSkill = (PlayerCastingSkill)this.skillStateMachine.GetSender ();
			playerCastingSkill.SetIsTriggering(true);
			playerCastingSkill.DecreaseMP ();
		}

		//取消标记技能图标
		public void UnMarkSkillImg()
		{
			PlayerCastingSkill playerCastingSkill = (PlayerCastingSkill)this.skillStateMachine.GetSender ();
			playerCastingSkill.IsMarkingOuterImg (false);
		}
     }
}