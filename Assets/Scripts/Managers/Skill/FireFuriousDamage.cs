using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Skill
{

	public class FireFuriousDamage : MonoBehaviour
	{
		public ISkillEntity skillEntity;

		public Vector3 centreOfAOECircle;

		//public float radiusSquare;

		Dictionary<int,GameObject> enemiesDic = new Dictionary<int,GameObject> ();

		float timer = 0f;

		void Start()
		{
			StartCoroutine ("FireFuriousCoroutine");
		}

		IEnumerator FireFuriousCoroutine()
		{
			float timerPerSecond = 0f;

			CollideEnemies ();

			while (true) {
				if(timerPerSecond >= 1f)
				{	
					//Debug.Log (timerPerSecond);
					timerPerSecond = 0f;
					if (timer > 10f) {
						Destroy (this);
						yield break;
					}
					//定时器时长从0秒开始，若已达到技能的持续时间，则停止统计
					if (timer < this.skillEntity.skillInfo.duration)
						CollideEnemies ();
					
					if (timer >= 1f && timer < 4f) {
						TakeFirstDamage ();
					} else if (timer >= 4f && timer < 10f) {
						TakeSecondDamage ();
					}
				}

				timerPerSecond += Time.deltaTime;

				timer += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}
		}

		void CollideEnemies ()
		{
			List<GameObject> enemyGOList = ObjectPooler.sharedInstance.GetPooledObjectsListByTag ("ZomBear|ZomBunny|Hellephant");

			foreach (GameObject enemyGO in enemyGOList) {

				int instanceID = enemyGO.transform.GetInstanceID ();
				GameObject enem;

				//若碰撞对象满足三个条件：
				//1.已激活。2.当前enemy对象在字典中找不到重复的对象。3.与玩家的距离在技能设定的有效攻击范围内。
				//则将当前enemy对象加入到字典中
				if (enemyGO.activeInHierarchy && !enemiesDic.TryGetValue (instanceID, out enem) && SkillManager.GetInstance().
					IsEnemyInCircle (this.centreOfAOECircle, Mathf.Pow ((this.skillEntity.skillInfo.range), 2), enemyGO.transform.position)) {

					enemiesDic.Add (instanceID, enemyGO);

				}
			}
		}

		void TakeFirstDamage ()
		{
			foreach (int key in enemiesDic.Keys) {

				GameObject enemyGO = enemiesDic [key];

				if (enemyGO.activeInHierarchy) {
					int damageValue = 0;

					if (enemiesDic.Count <= 6) {
						damageValue = this.skillEntity.skillInfo.intHP;

					} else {
						damageValue = (this.skillEntity.skillInfo.intHP * 6) / enemiesDic.Count;
					}

					enemyGO.GetComponent<EnemyHealth> ().TakeDamage (damageValue);
				}
			}
		}

		void TakeSecondDamage ()
		{
			foreach (int key in enemiesDic.Keys) {

				GameObject enemyGO = enemiesDic [key];

				if (enemyGO.activeInHierarchy) {
					int damageValue = 0;

					if (enemiesDic.Count <= 6) {
						damageValue = (int)(this.skillEntity.skillInfo.intHP * 0.05f);

					} else {
						damageValue = (int)((this.skillEntity.skillInfo.intHP * 6) / enemiesDic.Count * 0.05f);
					}

					enemyGO.GetComponent<EnemyHealth> ().TakeDamage (damageValue);

					//Debug.Log (enemyGO.GetComponent<EnemyHealth> ().CurrentHealth);
				}
			}
		}

		void OnDestroy ()
		{
			//在这里清除enemy对象的燃烧shader
		}
	
	}
}
