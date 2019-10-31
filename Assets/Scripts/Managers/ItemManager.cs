using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Item;

namespace SurvivalShooter.Manager
{
	public class ItemManager : MonoBehaviour
	{
		public static int CountOfItemNotPicked = 0;

		public Transform[] spawnPoints;

		List<ItemInfo> itemInfoList = new List<ItemInfo> ();

		int lastSpawnPointIndex = -1;

		// Use this for initialization
		void Start ()
		{
			FromJsonToObject ();

			SortItemListByDropRateDesc ();

			if (CountOfItemNotPicked < 3) {
				InvokeRepeating ("ProduceItem", 5f, 10f);
			}

		}

		//将物品配置文件（ItemInfo.json）中的JSON格式字符串转化为ItemInfo实例
		public void FromJsonToObject ()
		{
			//TextAsset该类是用来读取配置文件的
			//TextAsset只支持UTF-8
			TextAsset asset = Resources.Load ("ItemsInfo") as TextAsset;
			if (!asset)//读不到就退出此方法
				return;

			string strJson = asset.text;	

			if (strJson != null && !"".Equals (strJson)) {
				string[] strArrItemInfo = strJson.Split ('\n');

				foreach (string strItemInfo in strArrItemInfo) {
					ItemInfo itemInfo = JsonUtility.FromJson<ItemInfo> (strItemInfo);
					itemInfoList.Add (itemInfo);
				}
			}
		}

		//将ItemList的元素按照掉落率降序排列
		public void SortItemListByDropRateDesc ()
		{
			if (itemInfoList.Count > 1) {
				itemInfoList.Sort (delegate(ItemInfo itemInfo1, ItemInfo itemInfo2) {
					return itemInfo2.dropRate.CompareTo (itemInfo1.dropRate);
				});
			}
		}

		public void ProduceItem ()
		{
			float dropRate = Random.Range (0.0f, 1.0f);
			float lowerBound = 0f;
			float upperBound = 0f;

			foreach (ItemInfo itemInfo in itemInfoList) {
				lowerBound = upperBound;
				upperBound += itemInfo.dropRate;

				if (lowerBound <= dropRate && dropRate <= upperBound) {
					int spawnPointIndex = GenerateSpawnPointIndex ();
					GameObject treasureBoxGO = ObjectPooler.sharedInstance.GetPooledObjectByTag ("TreasureBox");

					if (treasureBoxGO != null) {
						treasureBoxGO.transform.position = spawnPoints [spawnPointIndex].position;
						treasureBoxGO.transform.rotation = spawnPoints [spawnPointIndex].rotation;

						treasureBoxGO.GetComponent<TreasureBoxHandler> ().itemInfo = itemInfo;

						treasureBoxGO.SetActive (true);

						CountOfItemNotPicked++;
						return;
					}
				}
			}
		}

		int GenerateSpawnPointIndex ()
		{
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);

			if (lastSpawnPointIndex == spawnPointIndex) {
				GenerateSpawnPointIndex ();
			}
			lastSpawnPointIndex = spawnPointIndex;

			return spawnPointIndex;
		}
	}
}
