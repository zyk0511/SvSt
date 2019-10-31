using UnityEngine;
using System.Collections;
using SurvivalShooter.Manager;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
	bool playerInRange;
    float timer;

	public void SetPlayerInRange(bool playerInRange)
	{
		playerInRange = playerInRange;
	}

	void OnEnable ()
    {
        player = PlayerManager.playerGO;
        playerHealth = player.GetComponent <PlayerHealth> ();
		anim = GetComponent <Animator> ();
		enemyHealth = GetComponent<EnemyHealth>();

		//如enemy对象在player贴身下被射杀，则失活后playerInRange仍为true，所以必须在激活时将playerInRange设置为false，避免其继续在攻击范围外对player触发攻击动作
		playerInRange = false;
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
		if(other.gameObject == player)
        {
			playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

		//enemyHealth = GetComponent<EnemyHealth>();
		//必须要判断enemy当前生命值是否大于0，如果enemy在player贴身的情况下被杀，playerInRange仍然为true
		if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.CurrentHealth > 0 && !enemyHealth.isDead)
        {
			//Debug.Log (playerInRange);
			Attack ();
        }

		if(playerHealth.CurrentHealth <= 0)
        {
			//anim.SetTrigger("PlayerDead");
			anim.SetBool("PlayerDead",true);
        }
    }


    void Attack ()
    {
        timer = 0f;

		if(playerHealth.CurrentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
