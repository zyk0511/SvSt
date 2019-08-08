using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace SurvialShoooter.Manager
{

	public class EnemyManager : MonoBehaviour
	{
		public PlayerHealth playerHealth;
		public GameObject enemy;
		public float spawnTime = 3f;
		public Transform[] spawnPoints;

		GameObject hudCanvasObj;

		void Start ()
		{
			hudCanvasObj = GameObject.Find("HUDCanvas");

			InvokeRepeating ("Spawn", 0f, spawnTime);
		}


		void Spawn ()
		{
			if (playerHealth.CurrentHealth <= 0) {
				return;
			}

			int spawnPointIndex = Random.Range (0, spawnPoints.Length);

			GameObject enemyObj = ObjectPooler.sharedInstance.GetPooledObjectByTag (enemy.tag);

//		Making a GameObject inactive will disable every component, 
//		turning off any attached renderers, colliders, rigidbodies, scripts, etc... 
//		Any scripts that you have attached to the GameObject will no longer have Update() called
			ReconfigureComponent (enemyObj);

			if (enemyObj != null) {
				enemyObj.transform.position = spawnPoints [spawnPointIndex].position;
				enemyObj.transform.rotation = spawnPoints [spawnPointIndex].rotation;

				enemyObj.SetActive (true);
			}

			//初始化NPC血条
			InstantiatingEnemyHPSlider (enemyObj.transform);

			//Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
		}


		void ReconfigureComponent (GameObject enemyObj)
		{
			Assembly assembly = Assembly.GetExecutingAssembly (); // 获取当前程序集 
			Object[] objectArr = new Object[1];
			objectArr [0] = enemyObj;

			assembly.CreateInstance ("Enemy." + enemy.tag, true, System.Reflection.BindingFlags.Default, null, objectArr, null, null); 

		}

		void InstantiatingEnemyHPSlider(Transform enemyObjTransform)
		{
			GameObject enemyHPSliderObj = ObjectPooler.sharedInstance.GetPooledObjectByTag ("EnemyHPSlider");

			enemyHPSliderObj.transform.SetParent (hudCanvasObj.transform);

			EnemyHPSliderPos enemyHPSliderPos = enemyHPSliderObj.transform.GetComponent<EnemyHPSliderPos> ();
			if(enemyHPSliderPos == null)
			{
				enemyHPSliderPos = enemyHPSliderObj.AddComponent<EnemyHPSliderPos> ();
			}
			enemyHPSliderPos.enemyObjFollow = enemyObjTransform;

			if ("ZomBear".Equals(enemyObjTransform.tag)) 
			{
				enemyHPSliderPos.offset.Set (0,50);
			}
			else if("ZomBunny".Equals(enemyObjTransform.tag))
			{
				enemyHPSliderPos.offset.Set (0,50);
			}
			else if("Hellephant".Equals(enemyObjTransform.tag))
			{
				enemyHPSliderPos.offset.Set (0,70);
			}

			enemyHPSliderObj.SetActive (true);
		}
	}
}