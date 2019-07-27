using UnityEngine;

namespace SurvialShoooter.Skill
{
    public class SkillStateMachine
    {
        ISkillState skillPreparingState;
        ISkillState skillSingingState;
        ISkillState skillReleasingState;
        ISkillState skillHittingState;

        ISkillEntity iSkillEntity;

        ISkillState skillState;

        public SkillStateMachine(ISkillEntity iSkillEntity)
        {
            skillPreparingState = new SkillPreparingState(this);
            skillSingingState = new SkillSingingState(this);
            skillReleasingState = new SkillReleasingState(this);
            skillHittingState = new SkillHittingState(this);

            //设置技能实体类
            this.iSkillEntity = iSkillEntity;

            //技能状态机初始化状态为skillPreparingState
            this.SetSkillState(skillPreparingState);
        }

        public ISkillState GetSkillPreparingState()
        {
            return this.skillPreparingState;
        }

        public ISkillState GetSkillSingingState()
        {
            return this.skillSingingState;
        }

        public ISkillState GetSkillReleasingState()
        {
            return this.skillReleasingState;
        }

        public ISkillState GetSkillHittingState()
        {
            return this.skillHittingState;
        }

        public ISkillEntity GetISkillEntity()
        {
            return this.iSkillEntity;
        }

        public void SetISkillEntity(ISkillEntity iSkillEntity)
        {
            this.iSkillEntity = iSkillEntity;
        }

        public void SetSkillState(ISkillState skillState)
        {
            this.skillState = skillState;
        }

        public ISkillState GetSkillState()
        {
            return this.skillState;
        }

        public void Sing()
        {
            skillState.Sing();
        }

        public void Release()
        {
            skillState.Release();
        }

        public void HitTarget()
        {
            skillState.HitTarget();
        }

        public void Complete()
        {
            skillState.Complete();
        }
    }
}