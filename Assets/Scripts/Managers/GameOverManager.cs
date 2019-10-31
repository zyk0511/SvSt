using UnityEngine;
using SurvivalShooter.Manager;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 5f;

    Animator anim;
	float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
		if (playerHealth.CurrentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
			restartTimer += Time.deltaTime;

			if (restartTimer >= restartDelay) {

				//SkillManager.skillEntity = null;

				Application.LoadLevel (Application.loadedLevel);
				PlayerManager.ResetMouseCursor ();

			}
        }
    }
}
