using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class PlayerMana : MonoBehaviour
{
	public static int startingMana = 220;
	private int currentMana;
	public Slider manaSlider;
	//MP自增速度(单位:秒)
	public float increasingSpeed = 1.0f;
	private Text manaPointText;

	private StringBuilder manaPointStrBuf = new StringBuilder();

	public int CurrentMana {
		get {
			return currentMana;
		}
	}

	void Awake()
	{
		currentMana = startingMana;
		manaPointText = manaSlider.transform.Find ("Fill Area").Find("Text").transform.GetComponent<Text>();

		AutoIncrement ();
	}

	public void UseMana (int amount)
	{
		currentMana -= amount;
		
		//manaSlider.value = currentMana;
		
		ChangeManaPointText ();
	}

	//随魔法值的变化动态改变魔法条数值
	public void ChangeManaPointText()
	{
		manaSlider.value = currentMana;
		manaPointStrBuf.Append (currentMana.ToString());
		manaPointStrBuf.Append ("/");
		manaPointStrBuf.Append (startingMana.ToString());
		manaPointText.text = manaPointStrBuf.ToString();
		manaPointStrBuf.Remove (0,manaPointStrBuf.Length);
	}

	void IncreaseMana()
	{
		if(currentMana == startingMana){
			return;
		}

		currentMana++;

		ChangeManaPointText ();
	}

	public void IncreaseMana(int amount)
	{
		currentMana += amount;

		if(currentMana > startingMana){
			currentMana = startingMana;
		}

		ChangeManaPointText ();
	}

	//自增魔法值
	public void AutoIncrement()
	{
		InvokeRepeating ("IncreaseMana",increasingSpeed,increasingSpeed);
	}

}