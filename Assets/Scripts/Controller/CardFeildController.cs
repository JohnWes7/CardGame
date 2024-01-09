using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFeildController : MonoBehaviour
{
    [SerializeField] private ShipController player;
    

    public void Init(ShipController player)
    {
        this.player = player;
    }

    public void UpdateShow(List<Card> handCard)
    {
        
    }
}
