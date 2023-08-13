using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 玩家数值
    [SerializeField] private int HP;
    [SerializeField] private int cost;


    // 玩家卡牌总共卡牌
    [SerializeField] private List<Card> mCards;

    // buff

    
    public PlayerController(int HP, List<Card> carryCards)
    {
        this.HP = HP;
        this.mCards = carryCards;

        // 洗牌

    }

    
}
