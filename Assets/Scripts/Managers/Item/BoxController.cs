using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SurvivalShooter.Item
{
	public class BoxController : MonoBehaviour, IDropHandler
	{
		/// <summary>
		/// 物品被放置的时候
		/// </summary>
		/// <param name="eventData"></param>
		public void OnDrop (PointerEventData eventData)
		{
			//被拖动的物体
			GameObject itemDragged = eventData.pointerDrag;
			//被拖动的物体只能是Item，否则不作相应
			if (!"Item".Equals (itemDragged.gameObject.tag)) {
				return;
			}

			//自身格子，被拖动物体取消射线检测，这里才能检测到
			GameObject itemByRaycast = eventData.pointerCurrentRaycast.gameObject;

			if ("Item".Equals (itemByRaycast.tag)) {
				//别拖动到存有物体的格子，交换位置
				Transform temp = itemDragged.GetComponent<ItemController> ().parent;
				itemDragged.transform.parent = itemByRaycast.transform.parent;
				itemByRaycast.transform.parent = temp;
				//
				itemByRaycast.transform.localPosition = Vector3.zero;
				itemDragged.transform.localPosition = Vector3.zero;
			} else if ("CountText".Equals (itemByRaycast.name)) {
				Transform temp = itemDragged.GetComponent<ItemController> ().parent;
				itemDragged.transform.parent = itemByRaycast.transform.parent.parent.parent;
				itemByRaycast.transform.parent.parent.parent = temp;

				itemByRaycast.transform.parent.parent.localPosition = Vector3.zero;
				itemDragged.transform.localPosition = Vector3.zero;
			} else if ("Box".Equals (itemByRaycast.name)) {
				//别拖动到空格子，设置位置
				itemDragged.transform.parent = transform;
				itemDragged.GetComponent<RectTransform> ().localPosition = Vector3.zero;
			}
		}
	}
}