using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using SurvivalShooter.Manager;

public class EnemyMovement : MonoBehaviour
{
	public float movingSpeed;
	Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
	Animator anim;
	bool isSlowedDown;

	void OnEnable ()
    {
        player = PlayerManager.playerGO.transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
		nav.speed = movingSpeed;

		anim = GetComponent<Animator> ();

		isSlowedDown = false;
    }

	public bool GetIsSlowedDown()
	{
		return this.isSlowedDown;
	}

    void Update ()
    {
		if(enemyHealth.CurrentHealth > 0 && playerHealth.CurrentHealth > 0)
        {
			nav.enabled = true;
			nav.SetDestination (player.position);
        }
        else
        {
            nav.enabled = false;
        }

		if(enemyHealth.CurrentHealth <= 0)
		{
			OpenSnowCoveringShader (false);
		}
    }

	public void FreezeEnemyForSeconds(float seconds)
	{
		IEnumerator coroutine = FreezeEnemyCoroutine (seconds);
		StartCoroutine (coroutine);
	}

	void OpenSnowCoveringShader(bool isOpen)
	{
		Material[] materails = transform.Find (gameObject.tag).GetComponent<Renderer> ().materials;
		if(materails.Length > 0)
		{
			if (isOpen) {
				materails [0].shader = Shader.Find ("Custom/SnowCovering");
			} else {
				materails [0].shader = Shader.Find ("Custom/Rim Lit Bumped Specular");
			}
		}
	}

	IEnumerator FreezeEnemyCoroutine(float seconds)
	{
		float timer = 0f;
		nav.speed = 0f;

		this.isSlowedDown = true;

		OpenSnowCoveringShader (true);
		anim.SetBool("PlayerDead",true);

		while (true) 
		{
			if(timer >= seconds)
			{
				//nav.enabled = true;
				nav.speed = movingSpeed;

				this.isSlowedDown = false;

				OpenSnowCoveringShader (false);
				anim.SetBool("PlayerDead",false);
				yield break;
			}

			timer += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		//yield return null;
	}

	public void SlowDownEnemyForSeconds(float seconds,float speed,ParticleSystem slowingDownParticle)
	{
		IEnumerator coroutine = SlowDownEnemyCoroutine (seconds,speed,slowingDownParticle);
		StartCoroutine (coroutine);
	}

	IEnumerator SlowDownEnemyCoroutine(float seconds,float speed,ParticleSystem slowingDownParticle)
	{
		float timer = 0f;
		//nav.enabled = false;
		nav.speed -= speed;

		if(slowingDownParticle != null)
		{
			slowingDownParticle.transform.parent = this.gameObject.transform;
			slowingDownParticle.transform.localPosition = Vector3.up;
			slowingDownParticle.Play ();
		}

		this.isSlowedDown = true;

		while (true) 
		{
			if(timer >= seconds)
			{
				//nav.enabled = true;
				nav.speed = movingSpeed;

				if(slowingDownParticle != null)
				{
					slowingDownParticle.Stop ();
					Destroy (slowingDownParticle);
				}

				this.isSlowedDown = false;

				yield break;
			}

			timer += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}
	}
}
