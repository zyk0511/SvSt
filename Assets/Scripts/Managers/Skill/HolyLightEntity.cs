using UnityEngine;
using System.Collections;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class HolyLightEntity : ISkillEntity
    {
		public HolyLightEntity(SkillInfo skillInfo):base(skillInfo){}

		public override void Sing()
        {
			base.Sing ();

			SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);
            
			Release ();
        }

		public override void Release()
        {
			base.Release ();
		}

		public override void HitTarget()
        {
			base.HitTarget ();
        }

		public override void Complete()
        {
			base.Complete ();
        }

		public override void Update()
		{
			
		}
    }
}
