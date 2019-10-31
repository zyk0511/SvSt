using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class ManaEntity : MonoBehaviour
	{
		public ItemInfo itemInfo;

		public void SetItemInfo(ItemInfo itemInfo)
		{
			this.itemInfo = itemInfo;
		}

		public void StartUsing(ItemUsing itemUsing)
		{
			if (itemUsing != null) {

				itemUsing.SetIsTriggering (true);

				itemUsing.itemController.ReduceItemCount ();

				PlayerManager.playerMana.IncreaseMana(itemUsing.itemInfo.intMP);
			}
		}
	}
}
