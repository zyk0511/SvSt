using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill{

	public class DivineShield : MonoBehaviour {

		public ISkillEntity skillEntity;
		PlayerHealth playerHealth;

		// Use this for initialization
		void Start () {
			playerHealth = transform.gameObject.GetComponent<PlayerHealth> ();

			if(skillEntity.skillInfo.duration > 0 && playerHealth != null)
			{
				skillEntity.PlaySkillAudio ();
				OpenDivineShield (skillEntity.skillInfo.duration);
			}
		}

		void OpenDivineShield(float duration)
		{
			IEnumerator coroutine = OpenDivineShieldCoroutine (duration);
			StartCoroutine (coroutine);
		}

		IEnumerator OpenDivineShieldCoroutine(float duration)
		{
			float timer = 0f;

			playerHealth.SetIsProtectedByDivineShield (true);

			while (true) 
			{
				timer += Time.deltaTime;

				if(timer > duration)
				{

					SkillManager.GetInstance ().StopParticle (this.skillEntity.skillInfo.singingParticle);

					playerHealth.SetIsProtectedByDivineShield (false);

					skillEntity.Complete ();

					Destroy (this);

					yield break;
				}

				yield return new WaitForEndOfFrame();
			}

		}
	}
}
