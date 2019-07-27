using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPSliderPos : MonoBehaviour {

	public Transform enemyObjFollow;
	public Vector2 offset;

	RectTransform rectTrans;
	EnemyHealth enemyHealth;
	Slider enemyHPSlider;

	void OnEnable()
	{
		enemyHPSlider = this.transform.GetComponent<Slider> ();
		enemyHealth = enemyObjFollow.gameObject.GetComponent<EnemyHealth> ();
		rectTrans = this.transform.GetComponent<RectTransform> ();
		//血条长度和生命值保持一致
		rectTrans.sizeDelta = new Vector2((float)enemyHealth.startingHealth,rectTrans.rect.height);
		enemyHPSlider.maxValue = enemyHealth.startingHealth;
	}

	// Update is called once per frame
	void Update () {

		if(enemyHealth.isDead)
		{
			gameObject.transform.Translate (-Vector3.up * Time.deltaTime);
			gameObject.SetActive (false);
			return;
		}
		Vector3 targetPos = enemyObjFollow.transform.position;
		Vector2 pos = RectTransformUtility.WorldToScreenPoint (Camera.main,targetPos);
		rectTrans.position = pos + offset;
		//transform.rotation = Camera.main.transform.rotation;

		enemyHPSlider.value = enemyHealth.CurrentHealth;

		//ShowHPCanvas ();

	}

	//若角色的坐标不在屏幕范围内，则隐藏角色的血条，反之则显示
	void ShowHPCanvas()
	{
		if (transform.position.x > Screen.width || transform.position.y > Screen.height || transform.position.x < 0 || transform.position.y < 0) {
			gameObject.SetActive (false);
		}
		else 
		{
			gameObject.SetActive (true);
		}
	}
}
