using UnityEngine;
using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class FireFuriousEntity : ISkillEntity
	{
		Vector3 centreOfAOECircle;

		public FireFuriousEntity (SkillInfo skillInfo) : base (skillInfo){}

		public override void Sing ()
		{
			base.Sing ();

			SkillManager.GetInstance().PlayParticle(this.skillInfo.singingParticle);

			Release ();

		}

		public override void Release ()
		{
			base.Release ();
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AOEIcon1");
		}

		public override void HitTarget ()
		{
			//计算技能图标圆心与右切点在世界空间的实际作用距离即作用半径
			float radiusSquare = SkillManager.GetInstance ().GetRadiusSquareOfSkillCircleInWorldSpace (100f, Input.mousePosition, this.centreOfAOECircle);

			//Debug.Log (radiusSquare);

			SkillManager.GetInstance ().DamageAllEnemiesInCircle (this.centreOfAOECircle, radiusSquare, this.skillInfo.intHP);

			base.HitTarget ();

			Complete ();
		}

		public override void Complete ()
		{
			base.Complete ();
		}

		public override void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {

				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				TriggerSkill ();

				//播放玩家释放技能时的动画
				PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

				//获取AOE图标中心的世界坐标
				this.centreOfAOECircle = SkillManager.GetInstance ().GetCentreOfSkillCircleInWorldSpace (100f, Input.mousePosition);

				//将releasingParticle对象的世界坐标设置为AOE图标中心的世界坐标
				this.skillInfo.releasingParticle.transform.position = this.centreOfAOECircle;

				//播放玩家释放技能的粒子效果
				SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);

				HitTarget ();

			}else if(Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")){
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}
		}
	}
}
