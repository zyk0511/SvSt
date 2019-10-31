using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class Periapt1Entity : MonoBehaviour
	{
		public ItemInfo itemInfo;
//		float r;
//		float g;
//		float b;
//		Color newColor = new Color (0f, 0f, 0f);
//		int previousHealth;
		ItemController itemController;
		int startingHealth;
		int itemCount;

		public void SetItemInfo(ItemInfo itemInfo)
		{
			this.itemInfo = itemInfo;
		}

		void Start()
		{
			this.startingHealth = PlayerManager.playerStatus.startingHealth;
			this.itemController = this.gameObject.GetComponent<ItemController> ();
//			this.previousHealth = PlayerManager.playerStatus.CurrentHealth;
//			this.r = PlayerManager.playerStatus.hitPointImg.color.r;
//			this.g = PlayerManager.playerStatus.hitPointImg.color.g;
//			this.b = PlayerManager.playerStatus.hitPointImg.color.b;
//
//			newColor.r = this.r;
//			newColor.g = this.g;
//			newColor.b = this.b;
		}

		// Use this for initialization
		void Update ()
		{
			if(this.itemController.itemCount != 0){
				this.itemCount = this.itemController.itemCount;
			}

			PlayerManager.playerStatus.startingHealth = this.startingHealth + 300 * this.itemCount;
			PlayerManager.playerStatus.healthSlider.maxValue = PlayerManager.playerStatus.startingHealth;

//			int currentHealth = PlayerManager.playerStatus.CurrentHealth;
//
//			if (this.previousHealth > PlayerManager.playerStatus.CurrentHealth) {
//				newColor.r = PlayerManager.playerStatus.hitPointImg.color.r + ((this.previousHealth - PlayerManager.playerStatus.CurrentHealth) * 1.0f) / PlayerManager.playerStatus.startingHealth;
//			} else if(this.previousHealth < PlayerManager.playerStatus.CurrentHealth){
//				if (newColor.r <= this.r) {
//					newColor.r = this.r;
//				} else {
//					newColor.r = PlayerManager.playerStatus.hitPointImg.color.r - ((PlayerManager.playerStatus.CurrentHealth - this.previousHealth) * 1.0f) / PlayerManager.playerStatus.startingHealth;
//				}
//			}
//
//			Debug.Log (newColor.r);
//
//			newColor.g = this.g * ((PlayerManager.playerStatus.CurrentHealth * 1.0f) / PlayerManager.playerStatus.startingHealth);
//			newColor.b = this.b * ((PlayerManager.playerStatus.CurrentHealth * 1.0f) / PlayerManager.playerStatus.startingHealth);
//
//			PlayerManager.playerStatus.hitPointImg.color = newColor;
//
//			this.previousHealth = currentHealth;
		}

		void OnDestroy ()
		{
			PlayerManager.playerStatus.startingHealth = this.startingHealth;
			PlayerManager.playerStatus.healthSlider.maxValue = PlayerManager.playerStatus.startingHealth;

			if(PlayerManager.playerStatus.CurrentHealth > PlayerManager.playerStatus.startingHealth)
			{
				PlayerManager.playerStatus.SetCurrentHealth(PlayerManager.playerStatus.startingHealth);
			}
		}
	}
}