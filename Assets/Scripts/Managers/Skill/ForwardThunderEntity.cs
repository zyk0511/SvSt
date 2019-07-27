using UnityEngine;
using System.Collections;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ForwardThunderEntity : ISkillEntity
    {
		public ForwardThunderEntity(SkillInfo skillInfo):base(skillInfo){}

        public override void Sing()
        {
			//Debug.Log(PlayerGameObject.playerStatus.IsDamaged());
            //若该技能可被打断且当前玩家正遭受攻击，则当前技能从吟唱状态回退到准备状态，不再执行下一步动作
            if (this.skillInfo.intCanBeStoppedOrNot == 1 && PlayerManager.playerStatus.GetIsDamaged())
            {
                this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillPreparingState());
                return;
            }

			SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);
            this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillSingingState());
        }

		public override void Release()
        {
            this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillReleasingState());
        }

		public override void HitTarget()
        {
            this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillHittingState());
        }

		public override void Complete()
        {
            this.skillStateMachine.SetSkillState(this.skillStateMachine.GetSkillPreparingState());
        }

		public override void Update()
		{
			
		}
    }
}