using UnityEngine;
using System.Collections;

namespace SurvivalShooter.Skill
{
    public class SkillSingingState : ISkillState
    {

        public SkillStateMachine skillStateMachine;

        public SkillSingingState(SkillStateMachine skillStateMachine)
        {
            this.skillStateMachine = skillStateMachine;
        }

        public void Sing()
        {
            Debug.Log("Can't Sing the skill once again in the SkillSinging State !");
        }

        public void Release()
        {
            this.skillStateMachine.GetISkillEntity().Release();
        }

        public void HitTarget()
        {
            Debug.Log("Can't Hit Target in the SkillSinging State!");
        }

        public void Complete()
        {
            Debug.Log("Can't Complete the skill in the SkillSinging State!");
        }
    }
}
