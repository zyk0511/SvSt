using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class TreasureBoxHandler : MonoBehaviour
	{
		public ItemInfo itemInfo;

		GameObject itemInfoPanelGO;

		public void OnMouseDown()
		{
			if (Input.GetMouseButtonDown (0)) {

				Transform itemPanelGOtransform = PlayerManager.itemPanelGO.transform;

				foreach (Transform boxTransform in itemPanelGOtransform) {

					if (boxTransform.childCount == 0)
						continue;

					Transform itemOuterTrans = boxTransform.GetChild (0);

					if ("Item".Equals(itemOuterTrans.tag)) {
						ItemController itemController = itemOuterTrans.GetComponent<ItemController> ();
						//若背包中存在同类物品，则只修改数量
						if (this.itemInfo.strEngName.Equals (itemController.itemInfo.strEngName)) {

							itemController.itemCount++;

							Transform itemInnerTrans = itemOuterTrans.Find ("Item");
							Transform countBGTrans = null;

							if (itemInnerTrans.childCount == 1) {
								countBGTrans = itemInnerTrans.GetChild (0);

								countBGTrans.localScale = Vector3.one;

								Transform countTextTrans = countBGTrans.Find ("CountText");
								Text countText = countTextTrans.gameObject.GetComponent<Text> ();
								countText.text = itemController.itemCount.ToString ();

								this.gameObject.SetActive(false);

								if (itemInfoPanelGO != null && itemInfoPanelGO.activeInHierarchy) {
									itemInfoPanelGO.SetActive (false);
									PlayerManager.ResetMouseCursor ();
								}
							}
							return;
						}
					}
				}

				foreach (Transform boxTransform in itemPanelGOtransform) {
					//若背包中不存在同类物品，则将此物品存放在空格中
					if(boxTransform.childCount == 0)
					{
						//GameObject itemOuterGO = Instantiate (Resources.Load ("Prefabs/Item") as GameObject);
						GameObject itemOuterGO = ObjectPooler.sharedInstance.GetPooledObjectByTag("Item");

						itemOuterGO.transform.parent = boxTransform;

						itemOuterGO.transform.localPosition = Vector3.zero;

						itemOuterGO.transform.localScale = Vector3.one;

						Image itemImg = itemOuterGO.GetComponent<Image> ();
						itemImg.sprite = Resources.Load (this.itemInfo.strImgName, typeof(Sprite)) as Sprite;
						itemImg.raycastTarget = true;

						//首次加入的物品不需要展示数量
						Transform countBGTrans = itemOuterGO.transform.Find ("Item/CountBG");
						countBGTrans.localScale = Vector3.zero;

						//若当前为技能类物品，将ItemUsing类挂载到物品对象下，点击物品可触发技能
						//if (this.itemInfo.intSkillOrBuff == 1) {
						ItemUsing itemUsing = itemOuterGO.AddComponent<ItemUsing> ();
						itemUsing.itemInfo = this.itemInfo;
						//}

						//挂载相应的物品功能类
						if (this.itemInfo.strEntityClassName != null && !"".Equals (this.itemInfo.strEntityClassName)) {
							Type entityClass = Type.GetType(this.itemInfo.strEntityClassName);
							itemOuterGO.AddComponent (entityClass);

							//将Type类对象作为键来获取组件，键必须和加入组件时匹配
							//Component itemEntityClass = itemOuterGO.GetComponent (entityClass);
							//itemEntityClass.SendMessage ("SetItemInfo", this.itemInfo, SendMessageOptions.RequireReceiver);

						}

						ItemController itemController = itemOuterGO.GetComponent<ItemController> ();

						itemController.enabled = true;

						itemController.itemInfo = this.itemInfo;

						itemController.itemCount++;


						itemOuterGO.SetActive (true);


						Transform itemInnerTrans = itemOuterGO.transform.Find ("Item");
						if (itemInnerTrans != null) {
							itemInnerTrans.gameObject.GetComponent<Image>().sprite = Resources.Load (this.itemInfo.strImgName, typeof(Sprite)) as Sprite;
						}

						this.gameObject.SetActive(false);

						if (itemInfoPanelGO != null && itemInfoPanelGO.activeInHierarchy) {
							itemInfoPanelGO.SetActive (false);
							PlayerManager.ResetMouseCursor ();
						}

						return;
					}
				}
			}
		}

		void OnMouseEnter()
		{
			PlayerManager.SetMouseCursor ("icons8-hand-32");

			itemInfoPanelGO = ObjectPooler.sharedInstance.GetPooledObjectByTag ("ItemInfoPanel");

			//RectTransform rectTrans = itemInfoPanelGO.transform.GetComponent<RectTransform>();
			itemInfoPanelGO.transform.SetPositionAndRotation(new Vector3(Input.mousePosition.x + 150f,
				Input.mousePosition.y,0f),Quaternion.Euler(new Vector3(0f,0f,0f))
			);

			itemInfoPanelGO.SetActive (true);

			Transform ItemPic = itemInfoPanelGO.transform.Find ("ItemPic");
			if (ItemPic != null) {
				ItemPic.gameObject.GetComponent<Image> ().sprite = Resources.Load (this.itemInfo.strImgName, typeof(Sprite)) as Sprite;
			}
			Transform itemName = itemInfoPanelGO.transform.Find ("ItemName");
			if (itemName != null) {
				itemName.gameObject.GetComponent<Text> ().text = this.itemInfo.strChnName;
			}
			Transform itemDescription = itemInfoPanelGO.transform.Find ("ItemDescription");
			if (itemDescription != null) {
				itemDescription.gameObject.GetComponent<Text> ().text = this.itemInfo.description;
			}

			itemInfoPanelGO.transform.parent = PlayerManager.HUDCanvasGO.transform;
		}

		void OnMouseExit()
		{
			if (itemInfoPanelGO == null || !itemInfoPanelGO.activeInHierarchy)
				return;

			PlayerManager.ResetMouseCursor ();

			//this.itemInfo = null;
			Transform ItemPic = itemInfoPanelGO.transform.Find ("ItemPic");
			if (ItemPic != null) {
				ItemPic.gameObject.GetComponent<Image> ().sprite = null;
			}
			Transform itemName = itemInfoPanelGO.transform.Find ("ItemName");
			if (itemName != null) {
				itemName.gameObject.GetComponent<Text> ().text = "";
			}
			Transform itemDescription = itemInfoPanelGO.transform.Find ("ItemDescription");
			if (itemDescription != null) {
				itemDescription.gameObject.GetComponent<Text> ().text = "";
			}
			itemInfoPanelGO.SetActive (false);
		}
	}
}