using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth;
    int currentHealth = 0;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
	Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
	SphereCollider sphereCollider;
	UnityEngine.AI.NavMeshAgent navMeshAgent;
	public bool isDead;
    bool isSinking;

	Image hitPointImg;
	//Color hpOriginalColor = new Color(22.0f/255,160.0f/255,93.0f/255,1.0f);
	Color healthColor;

	EnemyAttack enemyAttack;

	public int CurrentHealth {
		get {
			return currentHealth;
		}
	}

	void OnEnable ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
		rigidbody = GetComponent<Rigidbody> ();
		navMeshAgent = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();
		sphereCollider = GetComponent<SphereCollider> ();

		enemyAttack = GetComponent<EnemyAttack> ();

		currentHealth = startingHealth;
		isDead = false;


    }

	void Start()
	{
		
		//hitPointImg = healthSlider.fillRect.transform.GetComponent<Image>();
		//hitPointImg.color = new Color (22.0f / 255, 160.0f / 255, 93.0f / 255, 1.0f);
	}

//    void Update ()
//    {
//        if(isSinking)
//        {
//            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
//        }
//    }

    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
		//ChangeSliderColor (amount);

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }

	//AOE伤害调用
	public void TakeDamage (int amount)
	{
		if(isDead)
			return;

		enemyAudio.Play ();

		currentHealth -= amount;

		if(currentHealth <= 0)
		{
			Death ();
		}
	}


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

    }


    public void StartSinking ()
    {
		navMeshAgent.enabled = false;
		rigidbody.isKinematic = true;
       	isSinking = true;
        ScoreManager.score += scoreValue;
		//gameObject.transform.position.Set (0,-5,0);
		gameObject.transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		gameObject.SetActive(false);


		//Destroy (gameObject, 2f);
    }

	//随伤害值的增加将生命值滑动条的颜色由绿逐渐变红
	public void ChangeSliderColor(int amount)
	{
		healthColor = hitPointImg.color;

		//rgb值范围在[0,1]
		float colorVar = amount * 1.0f/startingHealth;

		healthColor.r += colorVar;
		healthColor.g -= colorVar;
		healthColor.b -= colorVar;
		hitPointImg.color = healthColor;
	}

}
