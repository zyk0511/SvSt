using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SurvialShoooter.Manager;

public class PlayerCastingSkill : MonoBehaviour
{

	//冷却时间
    public int cdTime;
	//吟唱时间
	//public int castTime;
    //技能魔法值
    public int manaPoint;
    //输入的键盘值
    public KeyCode keyCode;
	//是否触发此技能
	public bool isTriggering;
    //技能按钮名称
    public string strBtnName;
	//计时器变量
	float timer = 0f;
	//倒计时Text控件对象
	Text countdownTxt;
	//内层Image控件对象
	Image innerImg;
	//外层Image控件对象
	Image outerImg;
	PlayerMana playerMana;

	public void SetIsTriggering(bool isTriggering)
	{
		this.isTriggering = isTriggering;
	}

	// Use this for initialization
	void Start ()
	{
        strBtnName = transform.gameObject.name;

        countdownTxt = transform.Find ("Countdown").GetComponent<Text> ();

		innerImg = GetComponent<Image> ();
		outerImg = transform.parent.GetComponent<Image> ();
			
		playerMana = PlayerManager.playerGO.GetComponent<PlayerMana> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		BanningSkillByCondition ();

		if (Input.GetKeyDown (keyCode)) {
			CastingSkill ();
		}

		SetImgFillAmount ();
	}

    void OnMouseDown()
    {
        CastingSkill();
    }

    public void OnMouseOver()
    {
        //PlayerManager.SetMouseCursor();
    }

    public void OnMouseExit()
    {
        PlayerManager.ResetMouseCursor();
    }

	//是否高亮标记当前技能的背景图片：选中此技能时标记，技能触发时取消标记
	public void IsMarkingOuterImg(bool isReplacing)
	{
		string imgPath;

		if (isReplacing) {
			imgPath = "Tazo_2D/Icon/" + this.strBtnName + "_choosing";
		} else {
			imgPath = "Tazo_2D/Icon/" + this.strBtnName;
		}

		outerImg.sprite = Resources.Load (imgPath, typeof(Sprite)) as Sprite;
	}

	public void CastingSkill ()
	{
		//警告魔法值不足
		if (playerMana.CurrentMana < manaPoint) {
			InsufficientManaAlert ();
			return;
		}

		if (timer == 0f) {
			//高亮标记当前技能的外层背景图片
			IsMarkingOuterImg (true);
			//在此发消息给技能系统
            CBaseEvent cBaseEvent = new CBaseEvent(CEventType.RELEASE_SKILL,this);
			CEventDispacher.GetInstance().DispatchEvent(cBaseEvent);
        }
	}

	//设置技能图片的倒计时效果
	public void SetImgFillAmount ()
	{
		if (this.isTriggering) {
			timer += Time.deltaTime;
			float num = cdTime - timer;
			innerImg.fillAmount = num / cdTime;
			countdownTxt.text = ((int)num).ToString ();

			if (timer >= cdTime - 1) {
				timer = 0f;
				innerImg.fillAmount = 0f;
				countdownTxt.text = "";
				SetIsTriggering (false);
			}
		}
	}


	//减少当前角色的魔法值
	public void DecreaseMP ()
	{
		playerMana.UseMana (manaPoint);
	}

	//根据条件判断是否屏蔽技能
	public void BanningSkillByCondition ()
	{
		if (playerMana.CurrentMana < manaPoint)
		{
			BanningSkill (true);
		}
		else{
			//若所有技能中有部分技能处于未完成状态，则不能释放该技能
			if (!SkillManager.GetInstance ().IsAllSkillStateMachinesStopped ()) {
				BanningSkill (true);
			} else {
				BanningSkill (false);
			}

			if (!isTriggering) {
				BanningSkill (false);
			}
		}

	}

	//是否屏蔽当前技能
	public void BanningSkill(bool isBan)
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
