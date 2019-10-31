using UnityEngine;
using System.Collections;

namespace SurvivalShooter.Skill
{
    public class SkillPreparingState : ISkillState
    {

        public SkillStateMachine skillStateMachine;

        public SkillPreparingState(SkillStateMachine skillStateMachine)
        {
            this.skillStateMachine = skillStateMachine;
        }

        public void Sing()
        {
            //将业务逻辑委托给相关的技能实体类，并在其中修改状态
            this.skillStateMachine.GetISkillEntity().Sing();
        }

        public void Release()
        {
            Debug.Log("Can't Release the skill in the SkillPreparing State !");
        }

        public void HitTarget()
        {
            Debug.Log("Can't Hit Target in the SkillPreparing State!");
        }

        public void Complete()
        {
            Debug.Log("Can't Complete the skill in the SkillPreparing State!");
        }
    }
}
