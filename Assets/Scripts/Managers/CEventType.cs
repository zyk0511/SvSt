using UnityEngine;
using System.Collections;

public enum CEventType
{
    //GeneralEvent
    
    //游戏结束
    GAME_OVER,
    //游戏通关
    GAME_WIN,
    //游戏暂停
    GAME_PAUSE,
    //游戏继续
    GAME_RESUME,

    //StateEvent
    
    //受到攻击
    UNDER_ATTACK,
    //空闲
    IDLE,

    //ActionEvent
    
    //普攻
    GENERAL_ATTACK,
    //释放技能/Buff
    RELEASE_SKILL

}

