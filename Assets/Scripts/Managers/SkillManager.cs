using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using SurvialShoooter.Skill;

namespace SurvialShoooter.Manager
{
    public class SkillManager : MonoBehaviour
    {
		static SkillManager instance;

		public static ISkillEntity skillEntity = null;

        Dictionary<string, SkillStateMachine> playerSkillsInfo = new Dictionary<string, SkillStateMachine>();

        List<SkillInfo> skillInfoList = new List<SkillInfo>();
        static List<ISkillEntity> skillEntityList = new List<ISkillEntity>();


        public static SkillManager GetInstance()
        {
            if (instance == null)
                instance = new SkillManager();

            return instance;
        }

        //技能装配过程需要等各项资源在Awake方法中创建完毕后才可以开始，所以这里必须使用Start方法
        void Start()
        {
            AssemblePlayerSkillsInfo();

            //成功装配玩家技能信息后，添加技能的事件监听
            CEventDispacher.GetInstance().AddEventListener(CEventType.RELEASE_SKILL, StartSkillStateMachine);

        }

		void Update()
		{
			if (skillEntity != null) {
				skillEntity.Update ();
			}

//			if (!SkillManager.GetInstance ().IsParticlePlaying (this.skillInfo.singingParticle) &&
//				!SkillManager.GetInstance ().IsParticlePlaying (this.skillInfo.releasingParticle)) {
//
//				SkillManager.GetInstance ().SetParticleLightEnabled (this.skillInfo.singingParticle, false);
//				SkillManager.GetInstance ().SetParticleLightEnabled (this.skillInfo.releasingParticle, false);
//			}

		}

        //根据配置文件中的设定，装配玩家技能信息
        public void AssemblePlayerSkillsInfo()
        {
            //SkillInfo skillInfo1 = new SkillInfo();
            //skillInfo1.strEngName = "fire furious";
            //skillInfo1.strChnName = "火焰雨";
            //skillInfo1.strBtnName = "S_fire_furious";
            //skillInfo1.strEntityClassName = "FireFuriousEntity";
            //skillInfo1.strKeyCode = "E";
            //skillInfo1.intSkillOrBuff = 1;
            //skillInfo1.intHP = 100;
            //skillInfo1.intMP = 120;
            //skillInfo1.cdTime = 30;

            skillInfoList = FromJsonToObject();

            foreach (SkillInfo skillInfo in skillInfoList)
            {

                InstantiateParticleByName(skillInfo.strSingingParticle, out skillInfo.singingParticle);
                AddParticleToGameObject(skillInfo.singingParticle, PlayerManager.playerGO);

                InstantiateParticleByName(skillInfo.strReleasingParticle, out skillInfo.releasingParticle);
				//releasingParticle挂载到playerGO上的目的是为了方便管理
				//AddParticleToGameObject(skillInfo.releasingParticle, PlayerManager.playerGO);



                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                SkillInfo[] arrSkillInfo = new SkillInfo[1];
                arrSkillInfo[0] = skillInfo;
                ISkillEntity skillEntity = assembly.CreateInstance("SurvialShoooter.Skill." + skillInfo.strEntityClassName,
                    true, System.Reflection.BindingFlags.Default, null, arrSkillInfo, null, null) as ISkillEntity;
                //Debug.Log(skillEntity1.ToString());
                skillEntityList.Add(skillEntity);

                SkillStateMachine skillStateMachine = new SkillStateMachine(skillEntity);
                
                skillEntity.SetSkillStateMachine(skillStateMachine);

                playerSkillsInfo.Add(skillInfo.strBtnName, skillStateMachine);
            }

        }

        //启动技能状态机
        public void StartSkillStateMachine(CBaseEvent cBaseEvent)
        {
            if (cBaseEvent.eventType == CEventType.RELEASE_SKILL)
            {
                SkillStateMachine skillStateMachine;
                this.playerSkillsInfo.TryGetValue(cBaseEvent.GetSender().name, out skillStateMachine);

                if (skillStateMachine != null)
                {
					skillStateMachine.SetSender (cBaseEvent.GetSender());

					skillStateMachine.Sing();
                    //skillStateMachine.Release();
                    //skillStateMachine.HitTarget();
                    //skillStateMachine.Complete();
                }
            }
        }

		public bool IsAllSkillStateMachinesStopped()
		{
			foreach(ISkillEntity skillEntity in skillEntityList)
			{
				//Console.WriteLine (skillStateMachine.GetSkillState().ToString());
				if(!skillEntity.GetSkillStateMachine().GetSkillState().Equals(skillEntity.GetSkillStateMachine().GetSkillPreparingState()))
				{
					return false;	
				}
			}

			return true;
		}

        //开/关技能粒子效果的灯光
        public void SetParticleLightEnabled(ParticleSystem particleGO,bool enalbled)
        {
            if (particleGO != null)
            {
                Light[] lights = particleGO.gameObject.transform.GetComponentsInChildren<Light>();
                foreach (Light light in lights)
                {
                    light.enabled = enalbled;
                }
            }
        }

		public bool IsParticlePlaying(ParticleSystem particleGo)
		{
			if (particleGo != null && !particleGo.isStopped)
				return true;
			
			return false;
		}

        //根据粒子名称从库中找到相应粒子的prefab，并创建该粒子的实例
        public void InstantiateParticleByName(string particleName, out ParticleSystem particleGO)
        {
            particleGO = null;
            if (particleName != null && !"".Equals(particleName))
            {
                ParticleSystem particleSystem = Resources.Load("_SFB_Particle Package 1/" + particleName, typeof(ParticleSystem)) as ParticleSystem;

				particleGO = Instantiate(particleSystem);
                
				SetParticleLightEnabled(particleGO, false);

				//particleGO.playOnAwake = false;
                particleGO.Stop();
                
                //Debug.Log(particleGO);
            }
        }

        //初始化技能粒子对象，并将其挂载到相应的GameObject节点下(玩家GO节点只能挂载singingParticle和releasingParticle，Enemy GO节点下只能挂载targetParticle)
        public void AddParticleToGameObject(ParticleSystem particleGO,GameObject GO)
        {
            if (particleGO != null && GO != null)
            {

                //粒子的灯光默认开启状态被设置为false（否则在场景中会看到灯光），添加时需要手动设置为true
                //SetParticleLightEnabled(particleGO, true);

				//particleGO.gameObject.transform.SetParent(GO.transform.FindChild("Player").gameObject.transform);
				particleGO.gameObject.transform.SetParent (GO.transform.Find("mesh_root").gameObject.transform);
                particleGO.gameObject.transform.position.Set(0f, 0f, 0f);
                particleGO.gameObject.transform.rotation = Quaternion.identity;
                particleGO.gameObject.transform.localScale.Set(1f, 1f, 1f);
            }
        }


        //播放粒子效果
        public void PlayParticle(ParticleSystem particle)
        {
            if (particle != null)
            {
                
                particle.Stop();
				//开启粒子的光源
				SetParticleLightEnabled (particle, true);
				particle.Play();
				//关闭粒子的光源
				//SetParticleLightEnabled (particle, false);
            }
        }

		public void StopParticle (ParticleSystem particle)
		{
			if (particle != null)
			{
				particle.Stop();
				//关闭粒子的光源
				SetParticleLightEnabled (particle, false);
			}
		}

        //将技能配置文件（PlayerSkillsInfo.json）中的JSON格式字符串转化为SkillInfo实例
        public List<SkillInfo> FromJsonToObject()
        {
            StreamReader streamReader = new StreamReader("Assets/Scripts/Managers/Skill/PlayerSkillsInfo.json");
            string strJson = streamReader.ReadToEnd();

            streamReader.Close();

            if (strJson != null && !"".Equals(strJson))
            {
                string[] strArrSkillInfo = strJson.Split('\n');

                //Debug.Log(strJson);

                foreach (string strSkillInfo in strArrSkillInfo)
                {
                    SkillInfo skillInfo = JsonUtility.FromJson<SkillInfo>(strSkillInfo);
                    skillInfoList.Add(skillInfo);
                }
            }

            return skillInfoList;
        }

		public Vector3 GetCentreOfSkillCircleInWorldSpace (float radius, Vector3 mousePosition)
		{
			Vector3 centreOfSkillCircle = new Vector3 (mousePosition.x + radius, mousePosition.y - radius, 0f);

			RaycastHit skillRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (centreOfSkillCircle), out skillRaycastHit);

			//Debug.Log (centreOfAOEHit.point);

			return skillRaycastHit.point;
		}

		public RaycastHit GetAimIconRaycastHitInWorldSpace (float radius, Vector3 mousePosition,string tagName)
		{
			Vector3 centreOfSkillCircle = new Vector3 (mousePosition.x + radius, mousePosition.y - radius, 0f);

			Vector3 rightPointOfTangency = new Vector3 (centreOfSkillCircle.x + radius, centreOfSkillCircle.y, 0f);

			Vector3 bottomPointOfTangency = new Vector3 (centreOfSkillCircle.x, centreOfSkillCircle.y - radius, 0f);

			Vector3 leftPointOfTangency = new Vector3 (centreOfSkillCircle.x - radius, centreOfSkillCircle.y, 0f);

			Vector3 topPointOfTangency = new Vector3 (centreOfSkillCircle.x, centreOfSkillCircle.y + radius, 0f);

			RaycastHit aimIconRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (centreOfSkillCircle), out aimIconRaycastHit);

			if (tagName.Contains (aimIconRaycastHit.collider.tag))
				return aimIconRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (rightPointOfTangency), out aimIconRaycastHit);

			if (tagName.Contains (aimIconRaycastHit.collider.tag))
				return aimIconRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (bottomPointOfTangency), out aimIconRaycastHit);

			if (tagName.Contains (aimIconRaycastHit.collider.tag))
				return aimIconRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (leftPointOfTangency), out aimIconRaycastHit);

			if (tagName.Contains (aimIconRaycastHit.collider.tag))
				return aimIconRaycastHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (topPointOfTangency), out aimIconRaycastHit);

			if (tagName.Contains (aimIconRaycastHit.collider.tag))
				return aimIconRaycastHit;

			//Debug.Log (aimIconRaycastHit.collider.tag);

			return aimIconRaycastHit;
		}

		public float GetRadiusSquareOfSkillCircleInWorldSpace (float radius, Vector3 mousePosition, Vector3 centreOfAOEcircle)
		{
			Vector3 rightPointOfTangency = new Vector3 (mousePosition.x + radius, mousePosition.y, 0f);

			RaycastHit rightPointOfTangencyHit;

			Physics.Raycast (Camera.main.ScreenPointToRay (rightPointOfTangency), out rightPointOfTangencyHit);

			float radiusSquare = Mathf.Pow (rightPointOfTangencyHit.point.x - centreOfAOEcircle.x, 2) + Mathf.Pow (rightPointOfTangencyHit.point.z - centreOfAOEcircle.z, 2);

			return radiusSquare;
		}

		public bool IsEnemyInCircle (Vector3 centreOfAOECircle, float radiusSquare, Vector3 position)
		{
			if (Mathf.Pow (centreOfAOECircle.x - position.x, 2) + Mathf.Pow (centreOfAOECircle.z - position.z, 2) <= radiusSquare) {
				return true;
			}
			return false;
		}

		public void DamageAllEnemiesInCircle (Vector3 centreOfAOECircle, float radiusSquare,int skillIntHP)
		{
			List<GameObject> enemyGOList = GameObject.Find ("EnemyManager").transform.GetComponent<ObjectPooler> ().GetPooledObjectsList ();

			foreach (GameObject enemyGO in enemyGOList) {

				if (enemyGO.activeInHierarchy && IsEnemyInCircle (centreOfAOECircle, radiusSquare, enemyGO.transform.position)) {
					enemyGO.GetComponent<EnemyHealth> ().TakeDamage (skillIntHP);
				}
			}
		}

		//获取作用范围内指定数量的敌人，并按照与目标的直线距离升序排序
		public List<GameObject> GetEnemyListInRangeByAmount(Vector3 startPosition, int amount, int range)
		{
			List<GameObject> enemyGOList = GameObject.Find ("EnemyManager").transform.GetComponent<ObjectPooler> ().GetPooledObjectsList ();

			List<KeyValuePair<int,float>> enemyDistanceKYList = new List<KeyValuePair<int,float>> ();

			Dictionary<int,GameObject> enemyGOInCircleDic = new Dictionary<int,GameObject> ();

			List<GameObject> enemyGOInRangeByAmountList = new List<GameObject>();

			foreach(GameObject enemyGO in enemyGOList)
			{
				float distanceSquare = Mathf.Pow (enemyGO.transform.position.x - startPosition.x, 2) + Mathf.Pow (enemyGO.transform.position.z - startPosition.z, 2);

				if(enemyGO.activeInHierarchy && distanceSquare <= Mathf.Pow(range,2))
				{
					int key = enemyGO.GetInstanceID ();
					KeyValuePair<int,float> enemyDistanceKY = new KeyValuePair<int,float> (key,distanceSquare);
					enemyDistanceKYList.Add (enemyDistanceKY);

					enemyGOInCircleDic.Add (key, enemyGO);
				}
			}

			//按值升序排列
			enemyDistanceKYList.Sort(delegate(KeyValuePair<int,float> s1,KeyValuePair<int,float> s2){
				return s1.Value.CompareTo(s2.Value);
			});

			//删除排序后超出指定数量的敌人
			enemyDistanceKYList.RemoveRange (amount, enemyDistanceKYList.Count - amount);

			foreach(KeyValuePair<int,float> enemyDistanceKY in enemyDistanceKYList)
			{
				GameObject enemyInCircle;
				enemyGOInCircleDic.TryGetValue (enemyDistanceKY.Key, out enemyInCircle);

				if (enemyInCircle != null) {
					enemyGOInRangeByAmountList.Add (enemyInCircle);
				}
			}

			return enemyGOInRangeByAmountList;
		}

		//初始化并实例化闪电链组件
		public GameObject InstantiateChainLightning(Transform startTrans, Transform targetTrans)
		{
			GameObject chainLightning = Resources.Load ("Prefabs/ChainLightning") as GameObject;

			UVChainLightning uVChainLightning = chainLightning.GetComponent<UVChainLightning> ();
			uVChainLightning.start = startTrans;
			uVChainLightning.target = targetTrans;

			chainLightning = Instantiate (chainLightning);

			return chainLightning;
		}
    }
}