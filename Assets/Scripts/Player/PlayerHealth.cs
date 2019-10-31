using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 650;
	int currentHealth;
	int previousHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	//HP自增速度(单位:秒)
	public float increasingSpeed = 3.0f;
	public Image hitPointImg;
	Color healthColor = new Color(0f,0f,0f);

	Color startingHealthColor = new Color(0f,0f,0f);

	float startingR;
	float startingG;
	float startingB;

	Text hitPointText;
	StringBuilder hitPointStrBuf = new StringBuilder();

	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

	public void SetCurrentHealth(int currentHealth)
	{
		this.currentHealth = currentHealth;
	}

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
	PlayerMana playerMana;
    bool isDead;
	bool isDamaged;
	//当遭受敌方某些法术攻击（如眩晕、缠绕等）会使得玩家处于该状态，被中断的玩家如正在释放主动技能，则会被打断操作
	bool isSuspended;
	bool isProtectedByDivineShield;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
		playerShooting = GetComponentInChildren <PlayerShooting> ();
		playerMana = GetComponent<PlayerMana> ();
        currentHealth = startingHealth;

		previousHealth = currentHealth;

		hitPointImg = healthSlider.fillRect.transform.GetComponent<Image>();
		hitPointText = healthSlider.transform.Find ("Fill Area").Find("Text").transform.GetComponent<Text>();

		startingHealthColor = hitPointImg.color;

		this.startingR = hitPointImg.color.r;
		this.startingG = hitPointImg.color.g;
		this.startingB = hitPointImg.color.b;
		healthColor.r = startingR;
		healthColor.g = startingG;
		healthColor.b = startingB;

        this.isDead = false;
		SetIsDamaged (false);
		SetIsProtectedByDivineShield (false);

		AutoIncrement ();

    }


    void Update ()
    {
		ChangeBgColor ();

		ChangeSliderColor ();
    }

	public void ChangeBgColor()
	{
		if(this.isDamaged)
		{
			damageImage.color = flashColour;
			SetIsDamaged(false);
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
	}

    public bool GetIsDamaged()
    {
        return this.isDamaged;

    }

    public void SetIsDamaged(bool isDamaged)
    {
        this.isDamaged = isDamaged;
    }

	public bool GetIsSuspended()
	{
		return this.isSuspended;
	}

	public void SetIsSuspended(bool isSuspended)
	{
		this.isSuspended = isSuspended;
	}

	public bool GetIsProtectedByDivineShield()
	{
		return this.isProtectedByDivineShield;
	}

	public void SetIsProtectedByDivineShield(bool isProtectedByDivineShield)
	{
		this.isProtectedByDivineShield = isProtectedByDivineShield;
	}

    public void TakeDamage (int amount)
    {
		if(!GetIsProtectedByDivineShield())
		{
			SetIsDamaged(true);

			currentHealth -= amount;

			playerAudio.Play ();

			if (currentHealth <= 0 && !isDead) {
				currentHealth = 0;
				Death ();
			}

			//healthSlider.value = currentHealth;
			changeSliderValue ();
			//ChangeSliderColor (amount);
		}
    }

	//随生命值的变化动态改变生命条数值
	public void changeSliderValue()
	{
		healthSlider.maxValue = startingHealth;
		healthSlider.value = currentHealth;
		hitPointStrBuf.Append (currentHealth.ToString());
		hitPointStrBuf.Append ("/");
		hitPointStrBuf.Append (startingHealth.ToString());
		hitPointText.text = hitPointStrBuf.ToString();
		hitPointStrBuf.Remove (0,hitPointStrBuf.Length);

	}

	//随伤害值的增加将生命值滑动条的颜色由绿逐渐变红
	public void ChangeSliderColor(int amount)
	{
		healthColor = hitPointImg.color;

		//rgb值范围在[0,1]
		float colorVar = amount * 1.0f/startingHealth;

		healthColor.r += colorVar;

		healthColor.g -= colorVar;

		if(healthColor.g > startingHealthColor.g)
		{
			healthColor.g = startingHealthColor.g;
		}

		healthColor.b -= colorVar;

		if(healthColor.b > startingHealthColor.b)
		{
			healthColor.b = startingHealthColor.b;
		}

		hitPointImg.color = healthColor;
	}

	public void ChangeSliderColor()
	{
		int currentHealth = this.CurrentHealth;

		if (this.previousHealth > this.CurrentHealth) {
			healthColor.r = this.hitPointImg.color.r + ((this.previousHealth - this.CurrentHealth) * 1.0f) / this.startingHealth;
		} else if(this.previousHealth < this.CurrentHealth){
			if (healthColor.r <= this.startingR) {
				healthColor.r = this.startingR;
			} else {
				healthColor.r = this.hitPointImg.color.r - ((this.CurrentHealth - this.previousHealth) * 1.0f) / this.startingHealth;
			}
		}

		//Debug.Log (healthColor.r);

		healthColor.g = this.startingG * ((this.CurrentHealth * 1.0f) / this.startingHealth);
		healthColor.b = this.startingB * ((this.CurrentHealth * 1.0f) / this.startingHealth);

		this.hitPointImg.color = healthColor;

		changeSliderValue ();

		this.previousHealth = currentHealth;

	}

    public Animator GetAnimtor()
    {
        return this.anim;
    }

    void Death ()
    {
        isDead = true;

		//取消自增生命值和魔法值
		CancelInvoke ();
		playerMana.CancelInvoke ();

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;

    }

	void IncreaseHealth()
	{
		if(currentHealth == startingHealth){
			return;
		}

		currentHealth++;

		changeSliderValue ();

		//ChangeSliderColor (-1);
	}

	public void IncreaseHealth(int amount)
	{
		currentHealth += amount;

		if(currentHealth > startingHealth){
			currentHealth = startingHealth;
		}

		changeSliderValue ();
		//ChangeSliderColor (-amount);
	}

	//自增生命值
	public void AutoIncrement()
	{
		InvokeRepeating ("IncreaseHealth",0,increasingSpeed);
	}

    public void RestartLevel ()
    {
		//unity chan的角色动画与模型分离，如需给动画绑定事件，可至unity chan资源包的Animations文件夹下找到相应的动画片段。
		//目前此游戏的结束动画已在新建的动画片段GameOverClip中设置，所以此函数暂时废弃
		//Application.LoadLevel (Application.loadedLevel);
    }

}
