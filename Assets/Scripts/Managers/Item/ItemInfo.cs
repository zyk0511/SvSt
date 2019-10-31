using UnityEngine;

namespace SurvivalShooter.Item
{
	[System.Serializable]
	public class ItemInfo
	{
		public string strEngName;
		public string strChnName;
		public string strImgName;
		public int intSkillOrBuff;
		public string strEntityClassName;
		public float dropRate;
		public int intHP;
		public int intMP;
		public float cdTime;
		public float distance;
		public string description;
	}
}