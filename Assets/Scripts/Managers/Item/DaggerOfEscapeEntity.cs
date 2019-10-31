using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurvivalShooter.Manager;

namespace SurvivalShooter.Item
{
	public class DaggerOfEscapeEntity : MonoBehaviour
	{
		ItemUsing itemUsing;
		public ItemInfo itemInfo;

		public void SetItemInfo (ItemInfo itemInfo)
		{
			this.itemInfo = itemInfo;
		}

		public void StartUsing (ItemUsing itemUsing)
		{
			if (itemUsing != null) {
				this.itemUsing = itemUsing;
				//禁止射击
				PlayerManager.playerShooting.SetIsShooting (false);
				//修改鼠标指针样式为技能图标
				PlayerManager.SetMouseCursor ("AimSingle");
			}
		}

		void Update ()
		{
			if (this.itemUsing != null && Input.GetMouseButtonDown (0)) {
				//获取AOE图标投射在世界空间的射线
				RaycastHit aimIconHitPoint = SkillManager.GetInstance ().GetAimIconRaycastHitInWorldSpace (16f, Input.mousePosition, "");

				if (aimIconHitPoint.collider != null && "Floor".Equals (aimIconHitPoint.collider.tag)) {
					//Debug.Log (raycastHit.collider.name);
					PlayerManager.ResetMouseCursor ();

					this.itemUsing.SetIsTriggering (true);
					//跳刀没有使用次数的限制
					//this.itemUsing.itemController.ReduceItemCount ();

					//PlayerManager.playerGO.transform.forward = aimIconHitPoint.point.normalized;
					Quaternion newRotation = Quaternion.LookRotation(aimIconHitPoint.point.normalized);
					PlayerManager.playerGO.GetComponent<Rigidbody>().MoveRotation(newRotation);
					PlayerManager.playerGO.transform.position = aimIconHitPoint.point;

					this.itemUsing = null;

					//恢复射击
					PlayerManager.playerShooting.SetIsShooting (true);
				}

			} else if (Input.GetMouseButtonDown (1) || Input.GetButtonDown ("Horizontal") || Input.GetButtonDown ("Vertical") || Input.GetButtonDown ("Cancel")) {
				//恢复默认的鼠标指针样式
				PlayerManager.ResetMouseCursor ();
				//物品信息也要清空
				this.itemUsing = null;
			}
		}
	}
}