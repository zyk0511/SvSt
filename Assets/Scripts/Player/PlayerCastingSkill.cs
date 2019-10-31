using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text;
using UnityEngine.EventSystems;
using SurvivalShooter.Manager;
using SurvivalShooter.Skill;

public class PlayerCastingSkill : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
	public SkillInfo skillInfo;
	//冷却时间
	//public int cdTime;
	//吟唱时间
	//public int castTime;
	//技能魔法值
	//public int manaPoint;
	//输入的键盘值
	//public KeyCode keyCode;
	//是技能(1)还是buff/debuff(2)
	//public int intSkillOrBuff; 
	//技能按钮名称
	//string strBtnName;
	//是否触发此技能
	bool isTriggering;
	//计时器变量
	float timer = 0f;
	//倒计时Text控件对象
	Text countdownTxt;
	//内层Image控件对象
	Image innerImg;
	//外层Image控件对象
	Image outerImg;

	StringBuilder skillInfoText;

	PlayerMana playerMana;

	public void SetIsTriggering (bool isTriggering)
	{
		this.isTriggering = isTriggering;
	}

	// Use this for initialization
	void Start ()
	{
		//strBtnName = transform.gameObject.name;

		countdownTxt = transform.Find ("Countdown").GetComponent<Text> ();

		innerImg = GetComponent<Image> ();
		outerImg = transform.parent.GetComponent<Image> ();

		skillInfoText = new StringBuilder();
		SetSkillInfoText ();

		playerMana = PlayerManager.playerGO.GetComponent<PlayerMana> ();
	}

	void SetSkillInfoText()
	{
		skillInfoText.Append ("<b>");
		skillInfoText.Append (this.skillInfo.strChnName);
		skillInfoText.Append ("(<color=yellow>");
		skillInfoText.Append (this.skillInfo.strKeyCode);
		skillInfoText.Append ("</color>)</b>\n");
		skillInfoText.Append ("冷却时间");
		skillInfoText.Append (this.skillInfo.cdTime);
		skillInfoText.Append ("秒。消耗魔法值");
		skillInfoText.Append (this.skillInfo.intMP);
		skillInfoText.Append ("点。\n");
		skillInfoText.Append ("<b>技能描述：</b>");
		skillInfoText.Append (this.skillInfo.description);
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.isTriggering) {

			SetImgFillAmount ();

		} else {
			if (playerMana.CurrentMana < this.skillInfo.intMP) {
				
				BanningSkill (true);
				InsufficientManaAlert ();
				//return;
			} else {
				//若所有技能中有部分技能处于未完成状态，则不能释放该技能
				if (!SkillManager.GetInstance ().IsAllSkillStateMachinesStopped ()) {
					BanningSkill (true);
					return;
				}

				if (!isTriggering) {
					BanningSkill (false);
				}

				String strKeyCode = GetKeyDownCode ().ToString();

				if (strKeyCode.Equals(this.skillInfo.strKeyCode)) {
					CastingSkill ();
				}
			}
		}
	}

	public KeyCode GetKeyDownCode()
	{
		if(Input.anyKeyDown)
		{
			foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown (keyCode))
					return keyCode;
			}
		}

		return KeyCode.None;
	}

	public void TriggerSkillForClicking()
	{
		if(!this.isTriggering && playerMana.CurrentMana >= this.skillInfo.intMP && SkillManager.GetInstance ().IsAllSkillStateMachinesStopped ()){
			CastingSkill ();
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if(!this.isTriggering && playerMana.CurrentMana >= this.skillInfo.intMP && SkillManager.GetInstance ().IsAllSkillStateMachinesStopped ()){
			CastingSkill ();
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		
		PlayerManager.GetInstance ().ShowMessageCanvas (true, skillInfoText.ToString());
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		PlayerManager.GetInstance ().hideMessageCanvas ();
	}

	//是否高亮标记当前技能的背景图片：选中此技能时标记，技能触发时取消标记
	public void IsMarkingOuterImg (bool isReplacing)
	{
		string imgPath;

		if (isReplacing) {
			imgPath = "Tazo_2D/Icon/" + this.skillInfo.strBtnName + "_choosing";
		} else {
			imgPath = "Tazo_2D/Icon/" + this.skillInfo.strBtnName;
		}

		outerImg.sprite = Resources.Load (imgPath, typeof(Sprite)) as Sprite;
	}

	public void CastingSkill ()
	{
		//高亮标记当前技能的外层背景图片
		IsMarkingOuterImg (true);
		//在此发消息给技能系统
		CBaseEvent cBaseEvent = new CBaseEvent (CEventType.RELEASE_SKILL, this.gameObject);
		CEventDispacher.GetInstance ().DispatchEvent (cBaseEvent);
	}

	//设置技能图片的倒计时效果
	public void SetImgFillAmount ()
	{
		float num = this.skillInfo.cdTime - timer;
		timer += Time.deltaTime;
		innerImg.fillAmount = num / this.skillInfo.cdTime;
		countdownTxt.text = ((int)num).ToString ();

		if (timer >= this.skillInfo.cdTime - 1) {
			timer = 0f;
			innerImg.fillAmount = 0f;
			countdownTxt.text = "";
			SetIsTriggering (false);
		}
	}


	//减少当前角色的魔法值
	public void DecreaseMP ()
	{
		playerMana.UseMana (this.skillInfo.intMP);
	}

	//是否屏蔽当前技能
	public void BanningSkill (bool isBan)
	{
		if (isBan) {
			innerImg.fillAmount = 1.0f;
		} else {
			innerImg.fillAmount = 0f;
		}
			
	}

	//警告魔法值不足
	public void InsufficientManaAlert ()
	{
		//Debug.Log ("Insufficient Mana");
	}

}
