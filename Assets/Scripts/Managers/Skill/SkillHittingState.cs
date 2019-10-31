using UnityEngine;
using System.Collections;

namespace SurvivalShooter.Skill
{
    public class SkillHittingState : ISkillState
    {
        public SkillStateMachine skillStateMachine;

        public SkillHittingState(SkillStateMachine skillStateMachine)
        {
            this.skillStateMachine = skillStateMachine;
        }

        public void Sing()
        {
            Debug.Log("Can't Sing the skill once again in the SkillHitting State !");
        }

        public void Release()
        {
            Debug.Log("Can't Release the skill once again in the SkillHitting State !");
        }

        public void HitTarget()
        {
            Debug.Log("Can't Hit Target once again in the SkillHitting State !");
        }

        public void Complete()
        {
            this.skillStateMachine.GetISkillEntity().Complete();
        }
    }
}
