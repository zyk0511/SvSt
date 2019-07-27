using UnityEngine;
using System.Collections;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public abstract class ISkillEntity
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

		public virtual void Sing (){}

		public virtual void Release (){}

		public virtual void HitTarget (){}

		public virtual void Complete (){}

		public virtual void Update(){}
     }
}