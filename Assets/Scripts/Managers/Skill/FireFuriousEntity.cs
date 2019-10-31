using UnityEngine;
using System.Collections.Generic;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{
	public class FireFuriousEntity : ISkillEntity
	{
		Vector3 centreOfAOECircle;

		//float radiusSquare;

		Vector3 mousePosition;

		public FireFuriousEntity (SkillInfo skillInfo) : base (skillInfo){}

		public override void Sing ()
		{
			this.skillInfo.isIconMarking = true;
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AOEIcon1");
			SkillManager.skillEntity = this;

		}

		public override void Release ()
		{
			//播放玩家释放技能时的动画
			PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

			//将releasingParticle对象的世界坐标设置为AOE图标中心的世界坐标
			this.skillInfo.releasingParticle.transform.position = this.centreOfAOECircle;

			//计算技能图标圆心与右切点在世界空间的实际作用距离即作用半径
			//this.radiusSquare = SkillManager.GetInstance ().GetRadiusSquareOfSkillCircleInWorldSpace (100f, this.mousePosition, this.centreOfAOECircle);

			//Debug.Log (radiusSquare);

			//SkillManager.GetInstance ().DamageAllEnemiesInCircle (this.centreOfAOECircle, radiusSquare, this.skillInfo.intHP,"ZomBear|ZomBunny|Hellephant");

			FireFuriousDamage fireFuriousDamage = this.skillInfo.releasingParticle.gameObject.AddComponent<FireFuriousDamage> ();
			fireFuriousDamage.skillEntity = this;
			fireFuriousDamage.centreOfAOECircle = this.centreOfAOECircle;
			//fireFuriousDamage.radiusSquare = this.radiusSquare;

			PlaySkillAudio ();

			//播放玩家释放技能的粒子效果
			SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);

			base.Release ();
		}

		public override void HitTarget ()
		{
			base.HitTarget ();
		}

		public override void Complete ()
		{
			base.Complete ();
		}

		public override void Update ()
		{
			if (IsSkillStopped ()) {
				Complete();
				return;
			}

			if (Input.GetMouseButtonDown (0)) {

				this.mousePosition = Input.mousePosition;

				RaycastHit skillRaycastHit = SkillManager.GetInstance ().GetCentreOfSkillCircleInWorldSpace (100f, this.mousePosition);

				if (skillRaycastHit.collider != null) {
					//获取AOE图标中心的世界坐标
					this.centreOfAOECircle = skillRaycastHit.point;

					//若大于施法距离则给玩家添加自动导航组件
					if(Vector3.Distance(PlayerManager.playerGO.transform.position,this.centreOfAOECircle) > this.skillInfo.distance)
					{
						PlayerManager.ResetMouseCursor ();

						NavigateToTarget (PlayerManager.playerGO.transform, this.centreOfAOECircle, this.skillInfo.distance,
							PlayerManager.playerGO.GetComponent<PlayerMovement> ().speed, this.StartSkillLogic);

						return;
					}

					StartSkillLogic ();
				}

			}else if(Input.GetMouseButtonDown (1) || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Cancel")){
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();

				Complete ();
			}

		}

		public void StartSkillLogic()
		{
			//恢复默认的鼠标指针样式
			PlayerManager.ResetMouseCursor ();

			base.Sing ();

			Release ();

			HitTarget ();

			Complete ();
		}
	}
}
