using UnityEngine;
//using System.Collections.Generic;

namespace SurvialShoooter.Skill
{
    [System.Serializable]
    public class SkillInfo
    {
        //技能英文名
        public string strEngName;
        //技能中文名
        public string strChnName;
        //对应按钮名称
        public string strBtnName;
        //对应技能实体类名
        public string strEntityClassName;
        //对应键盘快捷键
        public string strKeyCode;
        //是技能(1)还是buff/debuff(2)
        public int intSkillOrBuff;
        //是否可被打断(1是，0否)
        public int intCanBeStoppedOrNot;
        //技能伤害值
        public int intHP;
        //技能所耗魔法值
        public int intMP;
        //技能冷却时间
        public int cdTime;
        //技能吟唱时调用的动画名称
        public string strSingingAnimation;
        //技能吟唱时的粒子效果
        public string strSingingParticle;
        //singingParticle是否循环
        //public int intSingingParticleLoopOrNot;
        //技能释放时的粒子效果
        public string strReleasingParticle;
        //打击目标的粒子效果
        public string strTargetParticle;
		//是否针对多个目标
		public int intIsForMultiple;


        [System.NonSerialized]
        //技能吟唱时的粒子对象
        public ParticleSystem singingParticle;
        
        [System.NonSerialized]
        //技能释放时的粒子对象
        public ParticleSystem releasingParticle;
        [System.NonSerialized]
        //打击目标的粒子对象
        public ParticleSystem targetParticle;

    }
}