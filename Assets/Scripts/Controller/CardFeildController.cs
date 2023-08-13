using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFeildController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    

    public void Init(PlayerController player)
    {
        this.player = player;
    }

    public void UpdateShow(List<Card> handCard, )
    {
        
    }
}
