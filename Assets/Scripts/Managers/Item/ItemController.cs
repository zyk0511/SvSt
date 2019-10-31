using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class ItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[HideInInspector]
		public Transform parent;

		public ItemInfo itemInfo;
		public int itemCount;
		Transform grandParent;

		public void UpdateImageShow ()
		{
			//ItemInfoController info = GetComponent<ItemInfoController>();
			//GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/" + info.data.itemInfo.iconName);
		}

		public void UpdateNumShow ()
		{
//		if (GetComponent<ItemInfoController>().data.num <= -1)
//		{
//			Destroy(transform.gameObject);
//		}
//		num.text = transform.GetComponent<ItemInfoController>().data.num.ToString();
		}

		public void OnDrag (PointerEventData eventData)
		{
			transform.position = eventData.position;
			// 取消相对应的射线检测，拖动结束才能检测到底部格子而不被遮挡
			transform.GetComponent<Image> ().raycastTarget = false;
		}

		public void OnBeginDrag (PointerEventData eventData)
		{
			parent = transform.parent;
			grandParent = PlayerManager.HUDCanvasGO.transform;
			transform.parent = grandParent;
		}

		public void OnEndDrag (PointerEventData eventData)
		{
			//投放在屏幕空间的射线所碰撞到的物体对象
			GameObject itemByRaycastInScreen = eventData.pointerCurrentRaycast.gameObject;

			//若屏幕空间内未碰撞到对象，则仅允许物品投放在地上或边界上
			if (itemByRaycastInScreen == null) {
				Vector3 gameObjectPosition = new Vector3 (eventData.position.x, eventData.position.y, 0f);
				RaycastHit gameObjectRaycastHit;
				//投放在世界空间的射线所碰撞到的物体对象
				Physics.Raycast (Camera.main.ScreenPointToRay (gameObjectPosition), out gameObjectRaycastHit);

				if (gameObjectRaycastHit.collider != null && "Floor|LevelExtent".Contains (gameObjectRaycastHit.collider.gameObject.tag)) {
					//在地板上生成相应数量的宝箱
					for (int i = 0; i < this.itemCount; i++) {
						ProduceTreasureBoxOnTheFloor (gameObjectRaycastHit.point);
					}
					clearItemFromBox ();

					return;
				}	

			}

			//其他情况下，物品回到物品栏中原来所在的位置
			if (transform.parent == grandParent) {
				transform.parent = parent;
				transform.localPosition = Vector3.zero;
			}
			transform.GetComponent<Image> ().raycastTarget = true;
		}

		public void ReduceItemCount ()
		{
			Transform countBGTrans = transform.Find ("Item/CountBG");

			if (countBGTrans != null) {
				GameObject CountTextGO = countBGTrans.Find ("CountText").gameObject;
				Text countText = CountTextGO.GetComponent<Text> ();
				itemCount = int.Parse (countText.text);
				itemCount--;
				countText.text = itemCount.ToString ();

				if (itemCount < 1) {
					clearItemFromBox ();

				} else if (itemCount == 1) {
					countBGTrans.localScale = Vector3.zero;
				} else {
					countBGTrans.localScale = Vector3.one;
				}
			}
		}

		public void clearItemFromBox()
		{
			//若是技能类物品，需要删除ItemUsing组件
			//if (this.itemInfo.intSkillOrBuff == 1) {

			//将外层图片的fillAmount设置为0，防止出现扔掉物品导致冷却时间中断的情况
			Image itemOuterImg = this.gameObject.transform.Find ("Item").GetComponent<Image> ();
			itemOuterImg.fillAmount = 0;

			Destroy(this.gameObject.GetComponent<ItemUsing> ());
			//}

			//删除每个物品所对应的功能组件
			Destroy (this.gameObject.GetComponent(Type.GetType(this.itemInfo.strEntityClassName)));

			itemCount = 0;
			//将物品从背包的格子中剔除
			this.gameObject.transform.parent = null;
			this.gameObject.SetActive (false);

		}

		public void ProduceTreasureBoxOnTheFloor(Vector3 raycastHitPointPosition)
		{
			GameObject treasureBoxGO = ObjectPooler.sharedInstance.GetPooledObjectByTag ("TreasureBox");
			treasureBoxGO.transform.position = raycastHitPointPosition;
			treasureBoxGO.GetComponent<TreasureBoxHandler> ().itemInfo = itemInfo;
			treasureBoxGO.SetActive (true);
		}
	}
}