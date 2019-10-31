using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class ItemUsing : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
	{

		public ItemInfo itemInfo;
		public ItemController itemController;

		//是否触发此物品的技能
		bool isTriggering;
		float timer = 0f;
		//内层Image控件对象
		Image innerImg;

		StringBuilder itemInfoText;

		public void SetIsTriggering (bool isTriggering)
		{
			this.isTriggering = isTriggering;
		}

		// Use this for initialization
		void Start ()
		{
			innerImg = this.transform.Find("Item").GetComponent<Image> ();
			itemController = this.gameObject.GetComponent<ItemController> ();

			itemInfoText = new StringBuilder();
			SetItemInfoText ();
		}

		void SetItemInfoText()
		{
			itemInfoText.Append ("<b>");
			itemInfoText.Append (this.itemInfo.strChnName);
			itemInfoText.Append ("</b>\n");

			if(this.itemInfo.cdTime != 0){
				itemInfoText.Append ("冷却时间");
				itemInfoText.Append (this.itemInfo.cdTime);
				itemInfoText.Append ("秒。\n");
			}

			itemInfoText.Append ("<b>物品描述：</b>");
			itemInfoText.Append (this.itemInfo.description);
		}

		// Update is called once per frame
		void Update ()
		{
			if (this.isTriggering && itemController.itemCount > 0) {
				SetImgFillAmount ();
			}
		}

		//设置技能图片的倒计时效果
		public void SetImgFillAmount ()
		{
			float num = itemInfo.cdTime - timer;
			timer += Time.deltaTime;
			innerImg.fillAmount = num / itemInfo.cdTime;
			//countdownTxt.text = ((int)num).ToString ();

			if (timer >= itemInfo.cdTime - 1) {
				timer = 0f;
				innerImg.fillAmount = 0f;
				//countdownTxt.text = "";
				SetIsTriggering (false);
			}
		}

		public void OnPointerEnter (PointerEventData eventData)
		{

			PlayerManager.GetInstance ().ShowMessageCanvas (false, itemInfoText.ToString());
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			PlayerManager.GetInstance ().hideMessageCanvas ();
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			//当物品不是技能类时，无需响应鼠标点击事件
			if (this.itemInfo.intSkillOrBuff != 1)
				return;

			if(!isTriggering)
			{
				//CBaseEvent cBaseEvent = new CBaseEvent (CEventType.USE_ITEM, this.gameObject);
				//CEventDispacher.GetInstance ().DispatchEvent (cBaseEvent);
				if (this.itemInfo.strEntityClassName != null && !"".Equals (this.itemInfo.strEntityClassName)) {
					
					//将Type类对象作为键来获取组件，键必须和加入组件时匹配
					Component itemEntityClass = this.gameObject.GetComponent (Type.GetType(this.itemInfo.strEntityClassName));
					itemEntityClass.SendMessage ("StartUsing", this, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}
}