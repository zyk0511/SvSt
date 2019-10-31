using UnityEngine;
using System.Collections;
using SurvivalShooter.Manager;

public delegate void Action();

namespace SurvivalShooter.Skill
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
			//PlayerManager.playerStatus.GetAnimtor ().SetBool ("Run", false);

			//Debug.Log(PlayerManager.playerStatus.GetIsDamaged());

			//禁止射击
			PlayerManager.playerShooting.SetIsShooting (false);

			this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillSingingState());

			//Debug.Log(PlayerManager.GetCurrentAnimatorStateInfo ().IsName ("Base Layer.Idle"));
		}

		public virtual void Release (){
			
			TriggerSkill ();
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
		}

		public bool IsSkillStopped()
		{
			//PlayerCastingSkill playerCastingSkill = (PlayerCastingSkill)this.skillStateMachine.GetSender ();
			//playerCastingSkill.SetImgFillAmount ();

			//若该技能可被打断且当前玩家正遭受敌方的法术中断且当前技能为吟唱状态，则将回退到准备状态，不再执行下一步动作
			if (this.skillInfo.intCanBeStoppedOrNot == 1 && this.skillStateMachine.GetSkillState().Equals(this.skillStateMachine.GetSkillSingingState())
				&& PlayerManager.playerStatus.GetIsSuspended()) {
				//PlayerManager.playerStatus.GetAnimtor().SetTrigger("Idle");
				//这里只负责技能状态的切换，玩家对象的状态需由实际情况来决定
				//this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());

				return true;
			}

			return false;
		}

		//触发技能启动并扣除魔法值
		public void TriggerSkill()
		{
			GameObject skillBtnGO = (GameObject)this.skillStateMachine.GetSender ();
			PlayerCastingSkill playerCastingSkill = skillBtnGO.GetComponent<PlayerCastingSkill> ();
			playerCastingSkill.SetIsTriggering(true);
			playerCastingSkill.DecreaseMP ();
		}

		//取消标记技能图标
		public void UnMarkSkillImg()
		{
			GameObject skillBtnGO = (GameObject)this.skillStateMachine.GetSender ();
			PlayerCastingSkill playerCastingSkill = skillBtnGO.GetComponent<PlayerCastingSkill> ();
			playerCastingSkill.IsMarkingOuterImg (false);

			this.skillInfo.isIconMarking = false;
		}

		public void PlaySkillAudio()
		{
			GameObject skillBtnGO = (GameObject)this.skillStateMachine.GetSender ();
			skillBtnGO.GetComponent<AudioSource> ().Play ();
		}

		//给玩家添加自动导航组件，并完成既定动作
		public void NavigateToTarget(Transform startTrans,Vector3 targetPosition,float distance,float speed,Action action)
		{
			NavigatingToTarget navigatingToTarget = PlayerManager.playerGO.AddComponent<NavigatingToTarget> ();
			navigatingToTarget.startTrans = startTrans;
			navigatingToTarget.targetPosition = targetPosition;
			navigatingToTarget.distance = distance;
			navigatingToTarget.speed = speed;
			navigatingToTarget.action = action;
		}
     }
}