using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalShooter.Item
{
	public class HealingSalveCoroutine : MonoBehaviour
	{
		public ItemUsing itemUsing;
		float timer = 0f;

		void Start()
		{
			IEnumerator coroutine = HealingCoroutine (this.gameObject);
			StartCoroutine (coroutine);
		}

		IEnumerator HealingCoroutine (GameObject playerGO)
		{
			float timerPerSecond = 0f;

			int hitPoint = this.itemUsing.itemInfo.intHP / 45;

			PlayerHealth playerHealth = playerGO.GetComponent<PlayerHealth> ();
			//int playerCurrentPlayerHealth = playerHealth.CurrentHealth;

			while (true) {
				if (timerPerSecond >= 1f) {	
					//Debug.Log (timerPerSecond);
					timerPerSecond = 0f;
					//当超过持续时间或玩家受到NPC的伤害时，停止治疗
					if (timer > 45f || playerHealth.CurrentHealth == playerHealth.startingHealth ||playerHealth.GetIsDamaged()) {
						Destroy (this);
						yield break;
					}

					playerHealth.IncreaseHealth (hitPoint);
				}

				timerPerSecond += Time.deltaTime;

				timer += Time.deltaTime;

				yield return new WaitForEndOfFrame ();
			}
		}
	}
}