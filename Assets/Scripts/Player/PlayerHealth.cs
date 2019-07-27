using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class PlayerHealth : MonoBehaviour
{
    public static int startingHealth = 650;
	private int currentHealth;

    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	//HP自增速度(单位:秒)
	public float increasingSpeed = 3.0f;
	Image hitPointImg;
	Color healthColor = new Color(0f,0f,0f,0f);
	Color startinghealthColor = new Color(0f,0f,0f,0f);
	Text hitPointText;
	StringBuilder hitPointStrBuf = new StringBuilder();

	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
	PlayerMana playerMana;
    bool isDead;
    bool isDamaged;
	//表示是否处于中断状态。当遭受敌方某些法术攻击（如眩晕、缠绕等）会使得玩家处于该状态，被中断的玩家如正在释放主动技能，则会被打断操作
	bool isSuspended;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
		playerShooting = GetComponentInChildren <PlayerShooting> ();
		playerMana = GetComponent<PlayerMana> ();
        currentHealth = startingHealth;
		hitPointImg = healthSlider.fillRect.transform.GetComponent<Image>();
		hitPointText = healthSlider.transform.Find ("Fill Area").Find("Text").transform.GetComponent<Text>();

		startinghealthColor = hitPointImg.color;

        this.isDead = false;
        this.isDamaged = false;
		this.isSuspended = false;

		AutoIncrement ();

    }


    void FixedUpdate ()
    {
		ChangeBgColor ();
    }

	public void ChangeBgColor()
	{
		if(this.isDamaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

        SetIsDamaged(false);
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

    public void TakeDamage (int amount)
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
		ChangeSliderColor (amount);
    }

	//随生命值的变化动态改变生命条数值
	public void changeSliderValue()
	{
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

		if(healthColor.g > startinghealthColor.g)
		{
			healthColor.g = startinghealthColor.g;
		}

		healthColor.b -= colorVar;

		if(healthColor.b > startinghealthColor.b)
		{
			healthColor.b = startinghealthColor.b;
		}

		hitPointImg.color = healthColor;
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
		ChangeSliderColor (-1);
	}

	public void IncreaseHealth(int amount)
	{
		currentHealth += amount;

		if(currentHealth > startingHealth){
			currentHealth = startingHealth;
		}

		changeSliderValue ();
		ChangeSliderColor (-amount);
	}

	//自增生命值
	public void AutoIncrement()
	{
		InvokeRepeating ("IncreaseHealth",0,increasingSpeed);
	}

    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }

}
