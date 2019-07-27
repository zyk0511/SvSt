using UnityEngine;
using System.Collections.Generic;
using SurvialShoooter.Manager;

namespace SurvialShoooter.Skill
{
	public class ColdBarkEntity : ISkillEntity
	{

		public ColdBarkEntity (SkillInfo skillInfo) : base (skillInfo)
		{
		}

		public override void Sing ()
		{
			//Debug.Log(PlayerManager.playerStatus.GetAnimtor ().GetCurrentAnimatorStateInfo (PlayerManager.playerStatus.GetAnimtor ().GetLayerIndex("Base Layer")).IsName ("Base Layer.Idle"));
			//玩家释放技能前先回到idle状态
			PlayerManager.playerStatus.GetAnimtor ().SetBool ("Run", false);

			//Debug.Log(PlayerManager.playerStatus.GetIsDamaged());
			//若该技能可被打断且当前玩家正遭受敌方的法术中断，则当前技能从吟唱状态回退到准备状态，不再执行下一步动作
			if (this.skillInfo.intCanBeStoppedOrNot == 1 && PlayerManager.playerStatus.GetIsSuspended ()) {
				//PlayerManager.playerStatus.GetAnimtor().SetTrigger("Idle");
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
				return;
			}

			//Debug.Log(PlayerManager.GetCurrentAnimatorStateInfo ().IsName ("Base Layer.Idle"));

			//PlayerManager.playerStatus.GetAnimtor().SetBool("Run",false);

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillSingingState ());


			//Release ();

		}

		public override void Release ()
		{
			ReleaseSkill ();
			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillReleasingState ());
		}

		public override void HitTarget ()
		{

			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillHittingState ());

			Complete ();
		}

		public override void Complete ()
		{
			if (!SkillManager.GetInstance ().IsParticlePlaying (this.skillInfo.singingParticle) &&
			    !SkillManager.GetInstance ().IsParticlePlaying (this.skillInfo.releasingParticle)) {

				SkillManager.GetInstance ().SetParticleLightEnabled (this.skillInfo.singingParticle, false);
				SkillManager.GetInstance ().SetParticleLightEnabled (this.skillInfo.releasingParticle, false);
			}


			this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());
		}

		public override void Update ()
		{
			Vector3 mousePosition = Input.mousePosition;

			Debug.Log (this.GetSkillStateMachine().GetSkillState().ToString());

			if (Input.GetMouseButtonDown (0)) {
				
				//获取AOE图标投射在世界空间的射线
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition, "ZomBear|ZomBunny|Hellephant");

				//Debug.Log (aimIconHitPoint.y);

				if ("ZomBear".Equals (aimIconHitPoint.collider.tag) || "ZomBunny".Equals (aimIconHitPoint.collider.tag)
				    || "Hellephant".Equals (aimIconHitPoint.collider.tag)) {

					//Debug.Log (raycastHit.collider.name);

					PlayerManager.ResetMouseCursor ();

					this.skillInfo.releasingParticle.transform.position = PlayerManager.playerGO.transform.position;

					float angle = Vector3.Angle (aimIconHitPoint.point, PlayerManager.playerGO.transform.position);

					//Debug.Log (angle);

					PlayerManager.playerGO.transform.rotation = Quaternion.Euler (0f, angle, 0f);


					//播放玩家释放技能时的动画
					PlayerManager.playerStatus.GetAnimtor ().SetTrigger (this.skillInfo.strSingingAnimation);

					//this.skillInfo.releasingParticle.transform.position = Vector3.MoveTowards (this.skillInfo.releasingParticle.transform.position, aimIconHitPoint, 12.0f * Time.deltaTime);
					 
					this.skillInfo.releasingParticle.GetComponent<DemoParticleControl> ().targetPosition = aimIconHitPoint.point;

					SphereCollider sphereCollider = this.skillInfo.releasingParticle.gameObject.AddComponent<SphereCollider> ();
					sphereCollider.radius = 0.15f;

					ColdSkillDamage skillDamage = this.skillInfo.releasingParticle.gameObject.AddComponent<ColdSkillDamage> ();
					skillDamage.skillInfo = this.skillInfo;

					//播放玩家释放技能的粒子效果

					SkillManager.GetInstance ().PlayParticle (this.skillInfo.releasingParticle);

					//技能释放完毕后可进行射击操作
					PlayerManager.playerShooting.SetIsShooting (true);

					HitTarget ();

					SkillManager.skillEntity = null;
				}

			} else if (Input.GetMouseButtonDown (1)) {

				//取消释放技能的操作
				PlayerManager.ResetMouseCursor ();
				//恢复之前的魔法值
				PlayerManager.playerMana.IncreaseMana (this.skillInfo.intMP);
				//技能状态机回退到技能准备状态
				this.skillStateMachine.SetSkillState (this.skillStateMachine.GetSkillPreparingState ());

				SkillManager.skillEntity = null;
			}
		}

		void ReleaseSkill ()
		{
			//修改鼠标指针样式为技能图标
			PlayerManager.SetMouseCursor ("AimSingle");
			//禁止射击及释放任何其他技能
			PlayerManager.playerShooting.SetIsShooting (false);
			SkillManager.skillEntity = this;
		}
	}
}
