using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{

    private bool finished = true;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.S))
		{
			DoSkill();
		}
	}

    /// <summary>
    /// 释放技能
    /// </summary>
    public void DoSkill()
    {
         if(finished)
         {
             finished = false;
             StartCoroutine(SetSkillValue());
         }
    }

    /// <summary>
    /// 设置技能填充值
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetSkillValue()
    {
        for (int i = 0; i <= 100;i++ )
        {
            this.GetComponent<UnityEngine.UI.Image>().fillAmount = i*0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        finished = true;
    }
}
