using UnityEngine;
using System.Collections;

namespace SurvivalShooter.Skill
{
    public class SkillReleasingState : ISkillState
    {

        public SkillStateMachine skillStateMachine;

        public SkillReleasingState(SkillStateMachine skillStateMachine)
        {
            this.skillStateMachine = skillStateMachine;
        }

        public void Sing()
        {
            Debug.Log("Can't Sing the skill once again in the SkillReleasing State !");
        }

        public void Release()
        {
            Debug.Log("Can't Release the skill once again in the SkillReleasing State !");
        }

        public void HitTarget()
        {
            this.skillStateMachine.GetISkillEntity().HitTarget();
        }

        public void Complete()
        {
            Debug.Log("Can't Complete the skill in the SkillReleasing State!");
        }
    }
}