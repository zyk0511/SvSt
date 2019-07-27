using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using SurvialShoooter.Manager;

public class EnemyMovement : MonoBehaviour
{
	public float movingSpeed;
	Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
	Animator anim;

	IEnumerator coroutine;

	void OnEnable ()
    {
        player = PlayerManager.playerGO.transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		nav.speed = movingSpeed;

		anim = GetComponent<Animator> ();
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
    }

	public void FreezeEnemyForSeconds(float seconds)
	{
		coroutine = InterruptNavForSeconds (seconds);
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

	IEnumerator InterruptNavForSeconds(float seconds)
	{
		float timer = 0f;
		//nav.enabled = false;
		nav.speed = 0f;
		OpenSnowCoveringShader (true);
		anim.SetBool("PlayerDead",true);

		while (true) 
		{
			timer += Time.deltaTime;

			if(timer > seconds)
			{
				//nav.enabled = true;
				nav.speed = movingSpeed;
				OpenSnowCoveringShader (false);
				anim.SetBool("PlayerDead",false);
				yield break;
			}

			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}
}
